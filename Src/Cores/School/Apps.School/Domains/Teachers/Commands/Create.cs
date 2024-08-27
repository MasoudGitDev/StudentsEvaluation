using Domains.School.Abstractions;
using Domains.School.Teacher.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Models;

namespace Apps.School.Domains.Teachers.Commands;
public sealed record Create(string FirstName , string LastName , string PersonnelCode) : IRequest<Result> {
    public static Create New(string firstName , string lastName , string personnelCode)
        => new(firstName , lastName , personnelCode);
}

//================ handler
internal sealed class CreateTeacherHandler(ISchoolUOW _unitOfWork) : IRequestHandler<Create , Result> {
    public async Task<Result> Handle(Create request , CancellationToken cancellationToken) {
        var findTeacher = (await _unitOfWork.Queries.Teachers.GetByPersonnelCodeAsync(request.PersonnelCode));
        if(findTeacher is not null) {
            return Result.Failed($"The teacher with personnel code : <{request.PersonnelCode}> found.");
        }
        await _unitOfWork.CreateAsync(request.Adapt<Teacher>());
        await _unitOfWork.SaveChangesAsync();
        return Result.Success($"The new teacher with personnel code : <{request.PersonnelCode}> has been created successfully");
    }
}
