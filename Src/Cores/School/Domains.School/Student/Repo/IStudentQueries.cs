using StudentTbl = Domains.School.Student.Aggregate.Student;

namespace Domains.School.Student.Repo;

public interface IStudentQueries {
    Task<StudentTbl?> GetById(ulong id);
    Task<StudentTbl?> GetByNationalCode(string nationalCode);
    Task<List<StudentTbl>> GetAllAsync();
}
