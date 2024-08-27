using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using MediatR;
using Shared.Files.Extensions;
using Shared.Files.Models;
using Shared.Files.ValueObjects;

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
            .ThrowIfNull(Description.New($"No teacher found with " +
                $"personnel code: <{request.TeacherPersonnelCode}> to represent the course."));

        ( await FindCourseByCodeAsync(request.Code) )
            .ThrowIfNotNull(Description.New($"A course with code: <{request.Code}> already exists."));

        return await CreateAndSaveAsync(Course.New(request.Code , request.Name , teacher.Id) ,
            $"The new course with code: <{request.Code}> has been created successfully");
    }


}
