using StudentModel = Domains.School.Student.Aggregate.Student;
using CourseModel = Domains.School.Course.Aggregate.Course;

namespace Domains.School.ExamResult.Aggregate;
public partial class ExamResult {
    public static ExamResult New(
        ulong courseId ,
        ulong teacherId ,
        ulong studentId ,
        DateTime examDate ,
        float score)
        => new() {
            CourseId = courseId ,
            TeacherId = teacherId ,
            StudentId = studentId ,
            ExamDateTime = examDate ,
            Score = score
        };


    // for unit testing
    public static ExamResult New( ulong id,
       CourseModel course ,
       ulong teacherId ,
       ulong studentId ,
       DateTime examDate ,
       float score)
       => new() {
           Id = id ,
           CourseId = course.Id ,
           TeacherId = teacherId ,
           StudentId = studentId ,
           ExamDateTime = examDate ,
           Score = score ,
           Course = course
       };

    public static ExamResult New(
    CourseModel course ,
    StudentModel student ,
    DateTime examDate ,
    float score)
    => new() {
        CourseId = course.Id ,
        TeacherId = course.TeacherId ,
        StudentId = student.Id ,
        ExamDateTime = examDate ,
        Score = score ,
        Course = course,
        Student = student        
    };
}
