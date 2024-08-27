using CourseModel=Domains.School.Course.Aggregate.Course;

namespace Domains.School.Course.Repo;
public interface ICourseQueries {
    Task<CourseModel?> GetByIdAsync(ulong id);
    Task<CourseModel?> GetByCodeAsync(string courseCode);
    Task<List<CourseModel>> GetAllAsync();

}
