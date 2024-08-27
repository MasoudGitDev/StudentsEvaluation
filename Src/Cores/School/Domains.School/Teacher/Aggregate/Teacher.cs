using Domains.School.Shared;
using CourseModel = Domains.School.Course.Aggregate.Course;
using ExamResultModel = Domains.School.ExamResult.Aggregate.ExamResult;

namespace Domains.School.Teacher.Aggregate;
public partial class Teacher : Person {
    public string PersonnelCode { get; private set; } = null!;

    //============relationships
    public ICollection<CourseModel> Courses { get; private set; }
}
