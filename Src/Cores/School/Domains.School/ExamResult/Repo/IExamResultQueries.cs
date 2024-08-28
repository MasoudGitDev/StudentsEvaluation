using ExamResultModel = Domains.School.ExamResult.Aggregate.ExamResult;

namespace Domains.School.ExamResult.Repo;
public interface IExamResultQueries {
    Task<ExamResultModel?> GetByIdAsync(ulong id);
    Task<ExamResultModel?> HadStudentAnyExamAsync(ulong studentId , ulong courseId , DateTime ExamDateTime);
    Task<List<ExamResultModel>> GetAllAsync();
    Task<List<ExamResultModel>> GetStudentExamsAsync(ulong studentId);
}
