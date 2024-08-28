using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using MediatR;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Apps.School.Domains.Teachers.Queries;
public sealed record GetTeachers(PaginationDto Model) : IRequest<Result<List<TeacherDto>>> {
    public static GetTeachers New(PaginationDto model) => new(model);
}


//================handler
internal sealed class GetTeachersHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetTeachers , Result<List<TeacherDto>>>(_unitOfWork.MustHasValue()) {
    public override async Task<Result<List<TeacherDto>>> Handle(GetTeachers request , CancellationToken cancellationToken)
        => SuccessListResult(nameof(QueryPropertyNames.Teachers) , await GetTeacherDTOsAsync(request.Model));
}
