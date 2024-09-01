namespace Infra.SqlServerWithEF.Contexts;

/// <summary>
/// Just used in AppDbContextFactory
/// </summary>
internal class DbConnection {
    public static string DefaultConnectionString
        => @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StudentEvalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
}
