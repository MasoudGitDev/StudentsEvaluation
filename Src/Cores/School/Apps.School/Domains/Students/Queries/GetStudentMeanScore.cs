using Domains.School.Abstractions;
using MediatR;
using Shared.Files.DTOs;
using Shared.Files.Extensions;
using Shared.Files.Models;

namespace Apps.School.Domains.Students.Queries;
public sealed record GetStudentMeanScore(string NationalCode) : IRequest<Result<StudentMeanScoreDto>> {
}

//====================handler
internal sealed class GetStudentMeanScoreHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<GetStudentMeanScore , Result<StudentMeanScoreDto>>(_unitOfWork) {
    public override async Task<Result<StudentMeanScoreDto>> Handle(GetStudentMeanScore request , CancellationToken cancellationToken) {
        var student = ( await GetStudentByCodeAsync(request.NationalCode) )
            .ThrowIfNull(description: $"The Student with national code : <{request.NationalCode}> not found.");

        var averageScore = (await GetStudentExamsAsync(student.Id)).Average(x=> x.Score);

        return Result<StudentMeanScoreDto>.Success("Ok" ,
            new StudentMeanScoreDto(student.FirstName , student.LastName , student.NationalCode , averageScore));

    }
}
