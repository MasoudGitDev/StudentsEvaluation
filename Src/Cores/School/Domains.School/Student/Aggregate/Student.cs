using Domains.School.Shared;
using StudentCourseModel = Domains.School.StudentCourse.Aggregate.StudentCourse;
using ExamResultModel = Domains.School.ExamResult.Aggregate.ExamResult;

namespace Domains.School.Student.Aggregate;
public partial class Student : Person {
    public string NationalCode { get; private set; } = null!;


    //================relationships
    public ICollection<StudentCourseModel> Courses { get; set; }
    public ICollection<ExamResultModel> Exams { get; set; }
}

