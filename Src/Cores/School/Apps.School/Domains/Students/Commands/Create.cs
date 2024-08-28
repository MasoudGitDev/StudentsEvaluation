using Apps.School.Constants;
using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using Domains.School.Student.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Extensions;
using Shared.Files.Models;

namespace Apps.School.Domains.Students.Commands;
public sealed record Create(string FirstName , string LastName , string NationalCode) : IRequest<Result>;

//================ handler
internal sealed class CreateStudentHandler(ISchoolUOW _unitOfWork)
    : SchoolRequestHandler<Create , Result>(_unitOfWork.MustHasValue()) {
    public override async Task<Result> Handle(Create request , CancellationToken cancellationToken) {

        ( await GetStudentByCodeAsync(request.NationalCode) )
            .ThrowIfNotNull(MessageResults.FoundStudent , request.NationalCode);

        return await CreateAndSaveAsync(request.Adapt<Student>() , MessageResults.CreateStudent , request.NationalCode);
    }
}