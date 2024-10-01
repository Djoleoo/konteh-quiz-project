using FluentValidation;
using Konteh.BackOfficeApi.Configuration;
using Konteh.BackOfficeApi.Consumers;
using Konteh.BackOfficeApi.HubConfig;
using Konteh.Domain;
using Konteh.Infrastructure;
using Konteh.Infrastructure.ExeptionHandler;
using Konteh.Infrastructure.PiplineBehaviour;
using Konteh.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using System.Reflection;

namespace Konteh.BackOfficeApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));



        builder.Services.AddControllers();

        builder.Services.AddDbContext<KontehDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
        builder.Services.AddScoped<IRepository<Answer>, AnswerRepository>();
        builder.Services.AddScoped<IRepository<Exam>, ExamRepository>();
        builder.Services.AddScoped<IRepository<ExamQuestion>, ExamQuestionRepository>();
        builder.Services.AddScoped<IExamRepository, ExamRepository>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


        builder.Services.AddOpenApiDocument(o => o.SchemaSettings.SchemaNameGenerator = new CustomSwaggerSchemaNameGenerator());


        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddCors(options =>
        {
            var corsConfig = builder.Configuration.GetSection("CorsConfiguration").Get<CorsConfiguration>();
            if (corsConfig != null)
            {
                options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins(corsConfig.AllowedOriginBackOffice)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });

            }

        });

        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<ExamRequestedConsumer>();
            x.AddConsumer<ExamSubmittedConsumer>();
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
                    cfg.ReceiveEndpoint("exam-requested-queue", e =>
                    {
                        e.ConfigureConsumer<ExamRequestedConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("exam-submitted-queue", e =>
                    {
                        e.ConfigureConsumer<ExamSubmittedConsumer>(context);
                    });
                });
            }

        });

        builder.Services.AddSignalR();

        // Configure the HTTP request pipeline.

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigins");

        app.UseExceptionHandler();

        app.UseAuthentication();

        app.UseAuthorization();



        app.MapHub<ExamHub>("/examHub");

        app.MapControllers();

        app.UseOpenApi();
        app.UseSwaggerUi();

        app.Run();
    }
}