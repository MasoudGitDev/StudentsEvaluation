using Domains.School.Abstractions;
using Domains.School.ExamResult.Aggregate;
using Domains.School.StudentCourse.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Constants;
using Shared.Files.Extensions;
using Shared.Files.Models;

namespace Apps.School.Domains.ExamResults.Commands;
public sealed record Create(string TeacherPersonnelCode ,
    string CourseCode ,
    string StudentNationalCode ,
    float Score ,
    DateTime ExamDateTime
 ) : IRequest<Result> {
}

//==============handler
internal sealed class CreateExamResultHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<Create , Result>(_unitOfWork) {
    public override async Task<Result> Handle(Create request , CancellationToken cancellationToken) {
        var (personnelCode, courseCode, nationalCode, score, examDateTime) = request;

        examDateTime.MustDateTimeLessThanNow();
        score.MustScoreBeInRange();

        (ulong teacherId,ulong courseId,ulong studentId)= await CheckObjectsExistenceAsync(personnelCode , courseCode , nationalCode , examDateTime);
        var studentCourse = StudentCourse.New(studentId, courseId);
        await _unitOfWork.CreateAsync(studentCourse);

        return await CreateAndSaveAsync(ExamResult.New(courseId,teacherId,studentId , examDateTime , score),
            MessageResults.CreateExamResult , []);
    }

    private async Task<(ulong TeacherId,ulong CourseId,ulong StudentId)> CheckObjectsExistenceAsync(string teacherPersonnelCode ,
        string courseCode ,
        string studentNationalCode ,
        DateTime examDateTime) {

        var teacher = ( await FindTeacherByCodeAsync(teacherPersonnelCode) )
            .ThrowIfNull(MessageResults.NotFoundTeacher , teacherPersonnelCode);

        var course = ( await FindCourseByCodeAsync(courseCode) )
            .ThrowIfNull(MessageResults.NotFoundCourse , courseCode);

        var student = ( await FindStudentByCodeAsync(studentNationalCode) )
            .ThrowIfNull(MessageResults.NotFoundStudent , studentNationalCode);

        ( await HadStudentAnyExamAsync(student.Id , course.Id , examDateTime) )
            .ThrowIfNotNull(MessageResults.OneExamPerCoursePerDay , string.Empty);
        return (teacher.Id, course.Id, student.Id);
    }

}