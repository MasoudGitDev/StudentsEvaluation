using Domains.School.Course.Repo;
using Domains.School.ExamResult.Repo;
using Domains.School.Student.Repo;
using Domains.School.Teacher.Repo;

namespace Domains.School.Abstractions;
public interface ISchoolQueries {
    public IStudentQueries Students { get; }
    public ICourseQueries Courses { get; }
    public ITeacherQueries Teachers { get; }
    public IExamResultQueries Exams { get; }
}
