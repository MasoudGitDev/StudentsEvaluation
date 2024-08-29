using Apps.School.Domains.Students.Commands;
using Domains.School.Abstractions;
using Domains.School.Student.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Files.Constants;
using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace UnTests.Apps.School.Students.Commands;
public class CreateStudentHandlerTest {
    private readonly Mock<ISchoolUOW> _unitOfWork = new();
    private readonly CreateStudentHandler _handler;
    public CreateStudentHandlerTest() {
        _handler = new(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Successfully_When_NationalCode_IsValid() {
        //Arrange
        Create request = new("Student_FN_1" , "Student_LN_1" , "Student_NC_1");


        _unitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(request.NationalCode))
            .ReturnsAsync((Student?) null);

        Student student = Student.New(request.FirstName, request.LastName,request.NationalCode);
        _unitOfWork.Setup(x => x.CreateAsync(It.IsAny<Student>()))
            .Callback<Student>(studentItem => studentItem = student);

        _unitOfWork.Setup(x => x.SaveChangesAsync());

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().BeEquivalentTo(Result.Success(String.Format(MessageResults.CreateStudent , request.NationalCode)));
        _unitOfWork.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Student_With_NationalCode_Exist() {
        //Arrange
        Create request = new("Student_FN_1" , "Student_LN_1" , "Student_NC_1");
        Student student = Student.New(request.FirstName, request.LastName,request.NationalCode);

        _unitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(request.NationalCode))
            .ReturnsAsync(student);

        //Act
        Func<Task<Result>> action = async()=>  await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().BeEquivalentTo(String.Format(MessageResults.FoundStudent , request.NationalCode));
        _unitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }
}
