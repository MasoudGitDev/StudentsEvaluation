using Domains.School.ExamResult.Aggregate;
using Domains.School.ExamResult.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Impls.School;
internal class ExamResultQueries(AppDbContext _dbContext) : IExamResultQueries {
    public async Task<List<ExamResult>> GetAllAsync() {
        return await _dbContext.ExamResults.AsNoTracking().ToListAsync();
    }

    public async Task<ExamResult?> GetByIdAsync(ulong id) {
        return await _dbContext.ExamResults.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ExamResult>> GetStudentExamsAsync(ulong studentId) {
        return await _dbContext.ExamResults
            .AsNoTracking()
            .Where(x=>x.StudentId== studentId)
            .ToListAsync();
    }
}
