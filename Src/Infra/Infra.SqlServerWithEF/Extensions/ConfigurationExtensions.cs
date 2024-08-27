using Microsoft.Extensions.Configuration;

namespace Infra.SqlServerWithEF.Extensions;
public static class ConfigurationExtensions {
    public static string GetDefaultConnectionString(this IConfiguration configuration) {
        return configuration.GetConnectionString("Default") 
            ?? throw new ArgumentNullException(nameof(configuration));
    }

    public static string TempDefaultConnectionString 
        => @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StudentEvalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
}
