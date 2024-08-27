using TeacherModel = Domains.School.Teacher.Aggregate.Teacher;

namespace Domains.School.Teacher.Repo;
public interface ITeacherQueries {
    Task<TeacherModel?> GetByIdAsync(ulong id);
    Task<TeacherModel?> GetByPersonnelCodeAsync(string personnelCode);
    Task<List<TeacherModel>> GetAllAsync();
}
