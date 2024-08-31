using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using Domains.School.Student.Aggregate;
using MediatR;
using Shared.Files.Constants;
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
        => await ListResultAsync(request.PaginationModel , request.IsDescending);

    //===================privates
    private async Task<Result<List<StudentMeanScoreDto>>> ListResultAsync(PaginationDto paginationModel , bool isDescending) {
        var allStudents = await _unitOfWork.Queries.Students.GetAllAsync(paginationModel,LoadingType.Eager);
        List<StudentMeanScoreDto> studentMeanScores = CalculateStudentsAverageScore(allStudents ,isDescending );
        return CheckItemsCount(allStudents.Count , studentMeanScores.Count , studentMeanScores);

    }
    private Result<List<StudentMeanScoreDto>> CheckItemsCount(int total ,
        int calculatedScores ,
        List<StudentMeanScoreDto> items) {
        string message = string.Format(MessageResults.Students_Average_Scores , calculatedScores , total);
        return calculatedScores == total ?
            Result<List<StudentMeanScoreDto>>.Success(message , items) :
            Result<List<StudentMeanScoreDto>>.Warning(message , items);
    }

    private List<StudentMeanScoreDto> CalculateStudentsAverageScore(List<Student> allStudents ,
        bool isDescending) {

        List<StudentMeanScoreDto>  studentsWithMeanScores = [];
        foreach(var student in allStudents) {
            var result = CalculateStudentAverageScore(student);
            if(!result.IsSuccessful) {
                continue;
            }
            studentsWithMeanScores.Add(result.Model!);
        }

        return isDescending ?
            [.. studentsWithMeanScores.OrderByDescending(dto => dto.AverageScore)] :
            [.. studentsWithMeanScores.OrderBy(dto => dto.AverageScore)];
    }
}