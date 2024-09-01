using Microsoft.Extensions.Configuration;

namespace Infra.SqlServerWithEF.Extensions;
public static class ConfigurationExtensions {
    public static string GetDefaultConnectionString(this IConfiguration configuration) {
        return configuration.GetConnectionString("Default") 
            ?? throw new ArgumentNullException(nameof(configuration));
    }  
}