using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using MediatR;
using Shared.Files.Constants;
using Shared.Files.DTOs;
using Shared.Files.Exceptions;
using Shared.Files.Extensions;
using Shared.Files.Models;
using System.ComponentModel.DataAnnotations;

namespace Apps.School.Domains.Teachers.Queries;
public sealed record GetTeacherPerformance(string PersonnelCode) : IRequest<Result<List<TeacherPerformanceDto>>> {
    public static GetTeacherPerformance New(string personnelCode) => new(personnelCode);
}

//======================handler
internal sealed class GetTeacherPerformanceHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetTeacherPerformance , Result<List<TeacherPerformanceDto>>>(_unitOfWork) {
    public override async Task<Result<List<TeacherPerformanceDto>>> Handle(GetTeacherPerformance request ,
        CancellationToken cancellationToken) {

        var findTeacher = (await FindTeacherByCodeAsync(request.PersonnelCode))
            .ThrowIfNull(MessageResults.NotFoundTeacher, request.PersonnelCode);

        return SuccessListResult(nameof(QueryPropertyNames.Courses) ,
            await CalculateEducationScoreAsync(findTeacher.Courses));
    }

    private static async Task<List<TeacherPerformanceDto>> CalculateEducationScoreAsync(ICollection<Course> teacherCourses) {
        var performanceTasks = teacherCourses.Select(async course => await CalcAverageCourseScore(course));
        return [.. ( await Task.WhenAll(performanceTasks) )];
    }
    private static Task<TeacherPerformanceDto> CalcAverageCourseScore(Course teacherCourse) {
        try {
            var studentsScore = (teacherCourse.Students.Select((x)=> x.Student.Exams.Average(x => x.Score)));
            return Task.FromResult(new TeacherPerformanceDto(teacherCourse.Code ,
                teacherCourse.Name ,
                studentsScore.Count() ,
                studentsScore.Average()));
        }
        catch(Exception ex) {
            throw new CustomException("CalcAverageCourseScoreError" , ex.Message);
        }
    }
}