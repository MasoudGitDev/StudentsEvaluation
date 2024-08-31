using Domains.School.Course.Aggregate;
using Domains.School.Course.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Files.DTOs;

namespace Infra.SqlServerWithEF.Impls.School;
internal class CourseQueries(AppDbContext _dbContext) : ICourseQueries {
    public async Task<List<Course>> GetAllAsync(PaginationDto model) {
        var courses = _dbContext.Courses.Include(x=>x.Teacher);
        if(model.UsePagination) {
            return await courses
                    .Skip(( model.PageNumber - 1 ) * model.PageSize)
                    .Take(model.PageSize)
                    .ToListAsync();
        }
        return await courses.ToListAsync();
    }

    public async Task<Course?> GetByCodeAsync(string courseCode) {
        return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Code == courseCode);
    }

    public async Task<Course?> GetByIdAsync(ulong id) {
        return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
    }
}
