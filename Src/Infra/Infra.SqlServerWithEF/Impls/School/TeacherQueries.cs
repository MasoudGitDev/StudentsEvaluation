using Domains.School.StudentCourse.Aggregate;
using Domains.School.Teacher.Aggregate;
using Domains.School.Teacher.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Files.DTOs;

namespace Infra.SqlServerWithEF.Impls.School;
internal class TeacherQueries(AppDbContext _dbContext) : ITeacherQueries {
    public async Task<List<Teacher>> GetAllAsync(PaginationDto model)
        => model.UsePagination ?
            await _dbContext.Teachers.Skip(( model.PageNumber - 1 ) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync() :
            await _dbContext.Teachers.ToListAsync();

    public async Task<Teacher?> GetByIdAsync(ulong id) {
        return await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Teacher?> GetByPersonnelCodeAsync(string personnelCode) {
        return await _dbContext.Teachers
            .Include(teacher => teacher.Courses)
            .ThenInclude(course => course.Exams)
            .FirstOrDefaultAsync(x => x.PersonnelCode == personnelCode);
    }
}
