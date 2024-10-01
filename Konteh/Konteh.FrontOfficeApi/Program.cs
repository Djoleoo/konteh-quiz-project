using FluentValidation;
using Konteh.Domain;
using Konteh.FrontOfficeApi.Configuration;
using Konteh.FrontOfficeApi.Features.Exams;
using Konteh.Infrastructure;
using Konteh.Infrastructure.BackgroundJobs;
using Konteh.Infrastructure.ExeptionHandler;
using Konteh.Infrastructure.PiplineBehaviour;
using Konteh.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Reflection;

namespace Konteh.FrontOfficeApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<KontehDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
        builder.Services.AddScoped<IRepository<ExamQuestion>, ExamQuestionRepository>();
        builder.Services.AddScoped<IRepository<Exam>, ExamRepository>();
        builder.Services.AddScoped<IRepository<Candidate>, CandidateRepository>();
        builder.Services.AddScoped<IRandomNumberGenerator, RandomNumberGenerator>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddMassTransit(x =>
        {
            var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQConfiguration").Get<RabbitMQConfiguration>();
            if (rabbitMQConfig != null)
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQConfig.Host, "/", c =>
                    {
                        c.Username(rabbitMQConfig.Username);
                        c.Password(rabbitMQConfig.Password);
                    });
                });
            }

        });


        builder.Services.AddOpenApiDocument(o => o.SchemaSettings.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());

        builder.Services.AddCors(options =>
        {
            var corsConfig = builder.Configuration.GetSection("CorsConfiguration").Get<CorsConfiguration>();
            if (corsConfig != null)
            {
                options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins(corsConfig.AllowedOriginFrontOffice)
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });

            }

        });

        builder.Services.AddQuartz(q =>
        {
            var jobKey = new JobKey(nameof(ExpiredExamsCleanerJob));
            q.AddJob<ExpiredExamsCleanerJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("ExpiredExamsCleanerJob-trigger")
                .WithSimpleSchedule(o =>
                    o.WithIntervalInHours(1)
                    .RepeatForever())
            );
        });
        builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);

        var app = builder.Build();

        app.UseCors("AllowSpecificOrigins");
        app.UseHttpsRedirection();

        app.UseExceptionHandler();

        app.UseAuthorization();

        app.MapControllers();

        app.UseOpenApi();
        app.UseSwaggerUi();

        app.Run();
    }
}