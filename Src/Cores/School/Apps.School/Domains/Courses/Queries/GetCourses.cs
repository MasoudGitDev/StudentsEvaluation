using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using MediatR;
using Shared.Files.Models;

namespace Apps.School.Domains.Courses.Queries;
public sealed record GetCourses() : IRequest<Result<List<Course>>> {
    public static GetCourses New() => new();
}

//====================handler
internal sealed class GetCoursesHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetCourses , Result<List<Course>>>(_unitOfWork) {
    public override async Task<Result<List<Course>>> Handle(GetCourses request , CancellationToken cancellationToken) {
        return SuccessListResult(nameof(QueryPropertyNames.Courses) , await GetCoursesAsync());
    }
}
