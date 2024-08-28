using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using MediatR;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Apps.School.Domains.Students.Queries;
public sealed record GetStudents(PaginationDto Model)
    : IRequest<Result<List<StudentDto>>> {
    public static GetStudents New(PaginationDto model) => new(model);
}


//================handler
internal sealed class GetStudentsHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetStudents , Result<List<StudentDto>>>(_unitOfWork.MustHasValue()) {
    public override async Task<Result<List<StudentDto>>> Handle(GetStudents request , CancellationToken cancellationToken)
        => SuccessListResult(nameof(QueryPropertyNames.Students) , await GetStudentDTOsAsync(request.Model));
}
