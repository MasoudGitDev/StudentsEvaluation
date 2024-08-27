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
}
