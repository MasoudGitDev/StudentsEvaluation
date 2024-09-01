using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.SqlServerWithEF.Contexts;
internal class AppDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext> {
    public AppDbContext CreateDbContext(string[] args) {
        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionBuilder.UseSqlServer(DbConnection.DefaultConnectionString);
        return new AppDbContext(optionBuilder.Options);
    }
}
