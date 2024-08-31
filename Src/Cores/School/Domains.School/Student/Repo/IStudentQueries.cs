using Shared.Files.Constants;
using Shared.Files.DTOs;
using StudentTbl = Domains.School.Student.Aggregate.Student;

namespace Domains.School.Student.Repo;

public interface IStudentQueries {
    Task<StudentTbl?> GetByIdAsync(ulong id);
    Task<StudentTbl?> GetByNationalCodeAsync(string nationalCode , LoadingType loadingType = LoadingType.Lazy);
    Task<List<StudentTbl>> GetAllAsync(PaginationDto model , LoadingType loadingType = LoadingType.Lazy);
}
