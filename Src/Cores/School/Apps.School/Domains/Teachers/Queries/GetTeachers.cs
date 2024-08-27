using Domains.School.Abstractions;
using Domains.School.Teacher.Aggregate;
using MediatR;
using Shared.Files.Models;

namespace Apps.School.Domains.Teachers.Queries;
public sealed record GetTeachers : IRequest<Result<List<Teacher>>> {
    public static GetTeachers New() => new();
}


//================handler
internal sealed class GetTeachersHandler(ISchoolUOW _unitOfWork) : IRequestHandler<GetTeachers , Result<List<Teacher>>> {
    public async Task<Result<List<Teacher>>> Handle(GetTeachers request , CancellationToken cancellationToken) {
        var teachers = await _unitOfWork.Queries.Teachers.GetAllAsync();
        string message = teachers.Count > 0
            ? $"{teachers.Count} teachers found."
            : "No teachers found.";
        return Result<List<Teacher>>.Success(message , teachers);
    }
}
