using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using Domains.School.Student.Aggregate;
using MediatR;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Apps.School.Domains.Students.Queries;
public sealed record GetStudents(bool UsePagination = true , int PageNumber = 1 , int PageSize = 50)
    : IRequest<Result<List<StudentDto>>> {
    public static GetStudents New(bool usePagination = true , int pageNumber = 1 , int pageSize = 50)
        => new(usePagination , pageNumber , pageSize);
}


//================handler
internal sealed class GetStudentsHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetStudents , Result<List<StudentDto>>>(_unitOfWork.MustHasValue()) {
    public override async Task<Result<List<StudentDto>>> Handle(GetStudents request , CancellationToken cancellationToken)
        => SuccessListResult(nameof(QueryPropertyNames.Students) ,
            await GetStudentsAsync(request.UsePagination , request.PageNumber , request.PageSize));
}
