using CourseModel = Domains.School.Course.Aggregate.Course;
using StudentModel = Domains.School.Student.Aggregate.Student;
using TeacherModel = Domains.School.Teacher.Aggregate.Teacher;

namespace Domains.School.ExamResult.Aggregate;
public partial class ExamResult {
    public ulong Id { get; set; }
    public ulong CourseId { get; set; }
    public ulong TeacherId { get; set; }  
    public ulong StudentId { get; set; }   

    public float Score { get; set; } // نمره بین 0 تا 20
    public DateTime ExamDate { get; set; } // تاریخ آزمون که باید در گذشته باشد

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    //===============
    public CourseModel Course { get; private set; }
    public TeacherModel Teacher { get; private set; }
    public StudentModel Student { get; private set; }
}
