using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using Domains.School.Teacher.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Extensions;
using Shared.Files.Models;

namespace Apps.School.Domains.Teachers.Commands;
public sealed record Create(string FirstName , string LastName , string PersonnelCode) : IRequest<Result>;

//================ handler
internal sealed class CreateTeacherHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<Create , Result>(_unitOfWork.MustHasValue()) {
    public override async Task<Result> Handle(Create request , CancellationToken cancellationToken) {

        ( await FindTeacherByCodeAsync(request.PersonnelCode) )
            .ThrowIfNotNull(description: $"A teacher with personnel code : <{request.PersonnelCode}> found.");

        return await CreateAndSaveAsync(request.Adapt<Teacher>() ,
            $"The new teacher with personnel code : <{request.PersonnelCode}> has been created successfully");
    }
}
