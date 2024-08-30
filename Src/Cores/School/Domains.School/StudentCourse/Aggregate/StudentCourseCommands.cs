namespace Domains.School.StudentCourse.Aggregate;
public partial class StudentCourse {
    public static StudentCourse New(ulong studentId , ulong courseId)
        => new() {
            StudentId = studentId ,
            CourseId = courseId
        };
    public static StudentCourse New(ulong id , ulong studentId , ulong courseId)
           => new() {
               Id = id ,
               StudentId = studentId ,
               CourseId = courseId
           };
}