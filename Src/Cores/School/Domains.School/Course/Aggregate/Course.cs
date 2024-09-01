using ExamResultModel = Domains.School.ExamResult.Aggregate.ExamResult;
using TeacherModel = Domains.School.Teacher.Aggregate.Teacher;

namespace Domains.School.Course.Aggregate;
public partial class Course {
    public ulong Id { get; private set; }
    public ulong TeacherId { get; private set; }

    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    //================Relationships
    public TeacherModel Teacher { get; set; }
    public ICollection<ExamResultModel> Exams { get; set; }
}
