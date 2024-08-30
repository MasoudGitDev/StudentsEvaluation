using CourseModel = Domains.School.Course.Aggregate.Course;
using StudentModel = Domains.School.Student.Aggregate.Student;

namespace Domains.School.StudentCourse.Aggregate;
public partial class StudentCourse {
    public ulong Id { get; private set; }
    public ulong StudentId { get; private set; }
    public ulong CourseId { get; private set; }

    //=========
    public StudentModel Student { get; set; }
    public CourseModel Course { get; set; }
}
