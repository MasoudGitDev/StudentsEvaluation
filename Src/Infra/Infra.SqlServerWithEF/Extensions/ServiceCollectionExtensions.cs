using Infra.SqlServerWithEF.Contexts;
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

        return services;
    }
}
