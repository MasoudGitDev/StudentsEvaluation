using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using MediatR;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Apps.School.Domains.Students.Queries;
public sealed record GetStudentsMeanScore(PaginationDto PaginationModel , bool IsDescending = true)
    : IRequest<Result<List<StudentMeanScoreDto>>> {
    public static GetStudentsMeanScore New(PaginationDto paginationModel , bool isDescending = true)
        => new(paginationModel , isDescending);
}

//===============handler
internal sealed class GetStudentsMeanScoreHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetStudentsMeanScore , Result<List<StudentMeanScoreDto>>>(_unitOfWork.MustHasValue()) {
    public override async Task<Result<List<StudentMeanScoreDto>>> Handle(GetStudentsMeanScore request , CancellationToken cancellationToken)
        => SuccessListResult(nameof(QueryPropertyNames.StudentMeanScores) ,
            await CalculateStudentsAverageScoreAsync(request.PaginationModel , request.IsDescending));

    private async Task<List<StudentMeanScoreDto>> CalculateStudentsAverageScoreAsync(
        PaginationDto paginationModel ,
        bool isDescending) {
        var allStudents = await _unitOfWork.Queries.Students.GetAllAsync(paginationModel);
        var studentMeanScores = await Task.WhenAll(allStudents.Select(async student =>
        {
            var averageScore = (await GetStudentExamsAsync(student.Id)).Average(x => x.Score);
            return new StudentMeanScoreDto(student.FirstName, student.LastName, student.NationalCode, averageScore);
        }));
        return isDescending ?
            [.. studentMeanScores.OrderByDescending(dto => dto.AverageScore)] :
            [.. studentMeanScores.OrderBy(dto => dto.AverageScore)];
    }
}
