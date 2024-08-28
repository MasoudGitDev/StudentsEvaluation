using Domains.School.Shared.Abstractions;

namespace Domains.School.Abstractions {
    public interface ISchoolUOW {
        public ISchoolQueries Queries { get; }

        public Task CreateAsync<TEntity>(TEntity entity) where TEntity : class, new();
        public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class, new();
        public Task SaveChangesAsync();
    }
}
