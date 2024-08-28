using Domains.School.Student.Aggregate;
using Domains.School.Student.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Files.DTOs;

namespace Infra.SqlServerWithEF.Impls.School;
internal sealed class StudentQueries(AppDbContext _dbContext) : IStudentQueries {
    public async Task<List<Student>> GetAllAsync(PaginationDto model)
        => model.UsePagination ?
            await _dbContext.Students.Skip(( model.PageNumber - 1 ) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync() :
            await _dbContext.Students.ToListAsync();

    public async Task<Student?> GetByIdAsync(ulong id) {
        return await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Student?> GetByNationalCodeAsync(string nationalCode) {
        return await _dbContext.Students.FirstOrDefaultAsync(x => x.NationalCode == nationalCode);
    }
}
