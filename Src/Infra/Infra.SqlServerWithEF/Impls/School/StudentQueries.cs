using Domains.School.Student.Aggregate;
using Domains.School.Student.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Impls.School;
internal sealed class StudentQueries(AppDbContext _dbContext) : IStudentQueries {
    public async Task<List<Student>> GetAllAsync(bool usePagination = true , int pageNumber = 1 , int pageSize = 50) {
        return usePagination ?
            await _dbContext.Students
                .Skip(( pageNumber - 1 ) * pageSize)
                .Take(pageSize)
                .ToListAsync() :
            await _dbContext.Students.ToListAsync();
    }

    public async Task<Student?> GetById(ulong id) {
        return await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Student?> GetByNationalCode(string nationalCode) {
        return await _dbContext.Students.FirstOrDefaultAsync(x => x.NationalCode == nationalCode);
    }
}
