using Domains.School.Student.Aggregate;
using Domains.School.Student.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Files.Constants;
using Shared.Files.DTOs;

namespace Infra.SqlServerWithEF.Impls.School;
internal sealed class StudentQueries(AppDbContext _dbContext) : IStudentQueries {
    public async Task<List<Student>> GetAllAsync(PaginationDto model , LoadingType loadingType = LoadingType.Lazy) {
        var students = _dbContext.Students.AsQueryable();
        if(loadingType == LoadingType.Eager) {
            students = students.Include(x => x.Exams);
        }
        if(model.UsePagination) {
            students = students
                .Skip(( model.PageNumber - 1 ) * model.PageSize)
                .Take(model.PageSize);
        }
       return await students.ToListAsync();
    } 

    public async Task<Student?> GetByIdAsync(ulong id) {
        return await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Student?> GetByNationalCodeAsync(string nationalCode,LoadingType loadingType = LoadingType.Lazy) {
        var students = _dbContext.Students.AsQueryable();
        if(loadingType == LoadingType.Eager) {
            students = students.Include(x => x.Exams);
        }
        return await students.FirstOrDefaultAsync(x => x.NationalCode == nationalCode);
    }
}
