using Domains.School.Abstractions;
using Domains.School.Student.Repo;
using Infra.SqlServerWithEF.Contexts;
using Infra.SqlServerWithEF.Impls.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.SqlServerWithEF.Extensions;
public static class ServiceCollectionExtensions {
    public static IServiceCollection AddInfraLayerServices(this IServiceCollection services) {

        var configuration= services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddDbContext<AppDbContext>(builder => {
            builder.UseSqlServer(configuration.GetDefaultConnectionString());
            builder.EnableSensitiveDataLogging();  
        });

        // school services
        services.AddScoped<IStudentQueries , StudentQueries>();
        services.AddScoped<ISchoolQueries , SchoolQueries>();
        services.AddScoped<ISchoolUOW , SchoolUnitOfWork>();
        return services;
    }
}
