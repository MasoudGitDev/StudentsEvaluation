﻿using Domains.School.Abstractions;
using MediatR;
using Shared.Files.Constants;
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
        var student = ( await FindStudentByCodeAsync(request.NationalCode,LoadingType.Eager) )
            .ThrowIfNull(MessageResults.NotFoundStudent , request.NationalCode);

        return CalculateStudentAverageScore(student);

    }
}
