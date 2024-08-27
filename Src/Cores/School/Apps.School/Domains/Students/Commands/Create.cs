using Domains.School.Abstractions;
using Domains.School.Student.Aggregate;
using MediatR;
using Shared.Files.Models;
using Mapster;

namespace Apps.School.Domains.Students.Commands;
public sealed record Create(string FirstName , string LastName , string NationalCode) : IRequest<Result> {
    public static Create New(string firstName , string lastName , string nationalCode)
        => new(firstName , lastName , nationalCode);
}

//================ handler
internal sealed class CreateStudentHandler(ISchoolUOW _unitOfWork) : IRequestHandler<Create , Result> {
    public async Task<Result> Handle(Create request , CancellationToken cancellationToken) {
        var findStudent = (await _unitOfWork.Queries.Students.GetByNationalCode(request.NationalCode));
        if(findStudent is not null) {
            return Result.Failed($"The student with national code : <{request.NationalCode}> found.");
        }
        await _unitOfWork.CreateAsync(request.Adapt<Student>());
        await _unitOfWork.SaveChangesAsync();
        return Result.Success($"The new student with national code : <{request.NationalCode}> has been created successfully");
    }
}
