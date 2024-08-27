namespace Domains.School.Abstractions {
    public interface ISchoolUOW {
        public ISchoolQueries Queries { get; }

        public Task CreateAsync<TEntity>(TEntity entity);
        public Task DeleteAsync<TEntity>(TEntity entity);
        public Task SaveChangesAsync();
    }
}
