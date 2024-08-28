using Apps.School.Constants;
using Domains.School.Abstractions;
using MediatR;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Apps.School.Domains.Courses.Queries;
public sealed record GetCourses(PaginationDto Model) : IRequest<Result<List<CourseDto>>> {
    public static GetCourses New(PaginationDto model) => new(model);
}

//====================handler
internal sealed class GetCoursesHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetCourses , Result<List<CourseDto>>>(_unitOfWork) {
    public override async Task<Result<List<CourseDto>>> Handle(GetCourses request , CancellationToken cancellationToken) {
        return SuccessListResult(nameof(QueryPropertyNames.Courses) , await GetCoursesAsync(request.Model));
    }
}
