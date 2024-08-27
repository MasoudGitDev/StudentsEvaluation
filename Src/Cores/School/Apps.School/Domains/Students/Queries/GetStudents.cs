using Domains.School.Student.Aggregate;
using Shared.Files.Models;
using MediatR;
using Domains.School.Abstractions;

namespace Apps.School.Domains.Students.Queries;
public sealed record GetStudents : IRequest<Result<List<Student>>> {
    public static GetStudents New() => new();
}


//================handler
internal sealed class GetStudentsHandler(ISchoolUOW _unitOfWork) : IRequestHandler<GetStudents , Result<List<Student>>> {
    public async Task<Result<List<Student>>> Handle(GetStudents request , CancellationToken cancellationToken) {
        var students = await _unitOfWork.Queries.Students.GetAllAsync();
        string message = students.Count > 0
            ? $"{students.Count} students found."
            : "No students found.";
        return Result<List<Student>>.Success(message , students);
    }
}
