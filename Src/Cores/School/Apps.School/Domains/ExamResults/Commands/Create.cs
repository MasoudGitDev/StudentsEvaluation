using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.ExamResult.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Extensions;
using Shared.Files.Models;

namespace Apps.School.Domains.ExamResults.Commands;
public sealed record Create(ulong StudentId ,
    ulong TeacherId ,
    ulong CourseId ,
    float Score ,
    DateTime ExamDateTime
    ) : IRequest<Result> {
}

//==============handler
internal sealed class CreateExamResultHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<Create , Result>(_unitOfWork) {
    public override async Task<Result> Handle(Create request , CancellationToken cancellationToken) {
        if(request.ExamDateTime >= DateTime.UtcNow) {
            return Result.Failed(MessageResults.ExamDateError);
        }
        if(request.Score is < 0 or > 20) {
            return Result.Failed(String.Format(MessageResults.InvalidScore,request.Score));
        }
        ( await HadStudentAnyExamAsync(request.StudentId , request.CourseId , request.ExamDateTime) )
             .ThrowIfNotNull(MessageResults.OneExamPerCoursePerDay , string.Empty);

        return await CreateAndSaveAsync(request.Adapt<ExamResult>() , MessageResults.CreateExamResult);
    }
}