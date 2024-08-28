using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Extensions;
using Shared.Files.Models;

namespace Apps.School.Domains.Courses.Commands;
public sealed record Create(string Code , string Name , string TeacherPersonnelCode) : IRequest<Result> {
    public static Create New(string code , string name , string teacherPersonnelCode)
        => new(code , name , teacherPersonnelCode);
}

//============== handler
internal sealed class CreateCourseHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<Create , Result>(_unitOfWork) {
    public override async Task<Result> Handle(Create request , CancellationToken cancellationToken) {

        var teacher = ( await FindTeacherByCodeAsync(request.TeacherPersonnelCode) )
            .ThrowIfNull(MessageResults.NotFoundTeacher,request.TeacherPersonnelCode);

        ( await FindCourseByCodeAsync(request.Code) )
            .ThrowIfNotNull(MessageResults.FoundCourse , request.Code);

        return await CreateAndSaveAsync(request.Adapt<Course>() , MessageResults.CreateCourse , request.Code);
    }
}
