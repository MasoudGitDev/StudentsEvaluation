using Domains.School.Abstractions;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.StudentCourse.Aggregate;
using MediatR;
using Shared.Files.Constants;
using Shared.Files.Exceptions;
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

        (ulong teacherId, ulong courseId, ulong studentId) = await CheckObjectsExistenceAsync(personnelCode , courseCode , nationalCode , examDateTime);
        var studentCourse = StudentCourse.New(studentId, courseId);
        await _unitOfWork.CreateAsync(studentCourse);

        return await CreateAndSaveAsync(ExamResult.New(courseId , teacherId , studentId , examDateTime , score) ,
            MessageResults.CreateExamResult , []);
    }

    //====================== privates
    private async Task<(ulong TeacherId, ulong CourseId, ulong StudentId)> CheckObjectsExistenceAsync(string teacherPersonnelCode ,
        string courseCode ,
        string studentNationalCode ,
        DateTime examDateTime) {

        var teacher = ( await FindTeacherByCodeAsync(teacherPersonnelCode) )
            .ThrowIfNull(MessageResults.NotFoundTeacher , teacherPersonnelCode);

        var course = ( await FindCourseByCodeAsync(courseCode) )
            .ThrowIfNull(MessageResults.NotFoundCourse , courseCode);

        var student = ( await FindStudentByCodeAsync(studentNationalCode,LoadingType.Eager) )
            .ThrowIfNull(MessageResults.NotFoundStudent , studentNationalCode);

        await HadStudentAnyExams(student , course.Id , examDateTime);

        return (teacher.Id, course.Id, student.Id);
    }

    private static Task HadStudentAnyExams(Student student , ulong courseId , DateTime examDatetime) {
        IEnumerable<ExamResult?> examResults = student.Exams.Select(examResult => {
            if(examResult.CourseId == courseId && examResult.ExamDateTime == examDatetime ){
                return examResult;
            }
            return default;
        });
        if(examResults.Any()) {
            throw new CustomException("CreationError" , MessageResults.OneExamPerCoursePerDay);
        }
        return Task.CompletedTask;
    }

}