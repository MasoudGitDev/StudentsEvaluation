using ExamResultModel = Domains.School.ExamResult.Aggregate.ExamResult;

namespace Domains.School.ExamResult.Repo;
public interface IExamResultQueries {
    Task<ExamResultModel?> GetByIdAsync(ulong id);
    Task<List<ExamResultModel>> GetAllAsync();
}
