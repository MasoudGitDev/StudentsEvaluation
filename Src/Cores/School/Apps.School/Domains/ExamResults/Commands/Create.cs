using Domains.School.Abstractions;
using Domains.School.ExamResult.Aggregate;
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

        await CheckObjectsExistenceAsync(personnelCode , courseCode , nationalCode , examDateTime);

        return await CreateAndSaveAsync(request.Adapt<ExamResult>() , MessageResults.CreateExamResult);
    }

    private async Task CheckObjectsExistenceAsync(string teacherPersonnelCode ,
        string courseCode ,
        string studentNationalCode ,
        DateTime examDateTime) {

        ( await FindTeacherByCodeAsync(teacherPersonnelCode) )
            .ThrowIfNull(MessageResults.NotFoundTeacher , teacherPersonnelCode);

        var course = ( await FindCourseByCodeAsync(courseCode) )
            .ThrowIfNull(MessageResults.NotFoundCourse , courseCode);

        var student = ( await FindStudentByCodeAsync(studentNationalCode) )
            .ThrowIfNull(MessageResults.NotFoundStudent , studentNationalCode);

        ( await HadStudentAnyExamAsync(student.Id , course.Id , examDateTime) )
            .ThrowIfNotNull(MessageResults.OneExamPerCoursePerDay , string.Empty);
    }

}