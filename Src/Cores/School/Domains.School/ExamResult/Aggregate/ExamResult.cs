using CourseModel = Domains.School.Course.Aggregate.Course;
using StudentModel = Domains.School.Student.Aggregate.Student;
using TeacherModel = Domains.School.Teacher.Aggregate.Teacher;

namespace Domains.School.ExamResult.Aggregate;
public partial class ExamResult {
    public ulong Id { get; private set; }

    public ulong StudentId { get; private set; }
    public ulong CourseId { get; private set; }
    public ulong TeacherId { get; private set; }  
     

    public float Score { get; private set; } // نمره بین 0 تا 20
    public DateTime ExamDateTime { get; private set; } // تاریخ آزمون که باید در گذشته باشد

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    //===============
    public StudentModel Student { get; private set; }
}
