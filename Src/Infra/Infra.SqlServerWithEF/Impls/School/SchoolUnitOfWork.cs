using Domains.School.Abstractions;
using Infra.SqlServerWithEF.Contexts;

namespace Infra.SqlServerWithEF.Impls.School;

internal sealed class SchoolUnitOfWork(ISchoolQueries _queries , AppDbContext _dbContext) : ISchoolUOW {
    public ISchoolQueries Queries => _queries;

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new() {
        await _dbContext.AddAsync(entity , new CancellationToken());
    }

    public Task DeleteAsync<TEntity>(TEntity entity) where TEntity: class , new(){
        _dbContext.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync() {
        await _dbContext.SaveChangesAsync();
    }
}
