using Konteh.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace Konteh.Tests;

public class CustomWebApplicationFactory<TProgram>
: WebApplicationFactory<TProgram> where TProgram : class
{

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

            var descriptor = services.Single(d => d.ServiceType == typeof(DbContextOptions<KontehDbContext>));
            services.Remove(descriptor);


            var dbConnectionDescriptor = services.FirstOrDefault(
            d => d.ServiceType ==
                typeof(DbConnection));
            if (dbConnectionDescriptor != null)
            {
                services.Remove(dbConnectionDescriptor);
            }

            services.AddDbContext<KontehDbContext>(options =>
            {
                options.UseSqlServer("Server=.;Database=KontehDBTest;Trusted_Connection=True;TrustServerCertificate=True;");
            });

            services.RemoveAll(typeof(IConfigureOptions<JwtBearerOptions>));

            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("Test", options => { });

            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<KontehDbContext>();
            db.Database.Migrate();
        });

    }

}
