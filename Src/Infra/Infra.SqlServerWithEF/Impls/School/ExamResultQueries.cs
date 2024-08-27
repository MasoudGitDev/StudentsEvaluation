using Domains.School.ExamResult.Aggregate;
using Domains.School.ExamResult.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Impls.School;
internal class ExamResultQueries(AppDbContext _dbContext) : IExamResultQueries {
    public async Task<List<ExamResult>> GetAllAsync() {
        return await _dbContext.ExamResults.ToListAsync();
    }

    public async Task<ExamResult?> GetByIdAsync(ulong id) {
        return await _dbContext.ExamResults.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ExamResult?> HadStudentAnyExamAsync(ulong studentId , ulong courseId , DateTime ExamDateTime) {
        return await _dbContext.ExamResults.FirstOrDefaultAsync(x => 
            x.StudentId == studentId && x.CourseId == courseId && x.ExamDateTime.Date == ExamDateTime.Date
        );
    }
}
