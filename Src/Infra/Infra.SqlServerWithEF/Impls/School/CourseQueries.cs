using Domains.School.Course.Aggregate;
using Domains.School.Course.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Impls.School;
internal class CourseQueries(AppDbContext _dbContext) : ICourseQueries {
    public Task<List<Course>> GetAllAsync() {
        return _dbContext.Courses.ToListAsync();
    }

    public async Task<Course?> GetByCodeAsync(string courseCode) {
        return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Code == courseCode);
    }

    public async Task<Course?> GetByIdAsync(ulong id) {
        return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
    }
}
