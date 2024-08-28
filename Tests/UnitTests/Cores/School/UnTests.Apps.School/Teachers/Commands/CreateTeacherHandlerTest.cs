using Apps.School.Constants;
using Apps.School.Domains.Teachers.Commands;
using Domains.School.Abstractions;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Mapster;
using Moq;
using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace UnTests.Apps.School.Teachers.Commands;
public class CreateTeacherHandlerTest {

    private readonly Mock<ISchoolUOW> _mockUnitOfWork = new();
    private readonly CreateTeacherHandler _handler;
    public CreateTeacherHandlerTest() {
        _handler = new(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Teacher_With_SuccessResult_If_NotExist_Before() {
        //Arrange
        Create request = new("Teacher_FN_1" , "Teacher_LN_1" , "Teacher_PC_1");

        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(request.PersonnelCode))
            .ReturnsAsync((Teacher?) null);

        Teacher expectedTeacher = request.Adapt<Teacher>();
        _mockUnitOfWork.Setup(x => x.CreateAsync(It.IsAny<Teacher>())).Callback<Teacher>(teacher =>
            teacher = expectedTeacher);

        _mockUnitOfWork.Setup(x => x.SaveChangesAsync());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        result.Should().BeEquivalentTo(Result.Success(String.Format(MessageResults.CreateTeacher , request.PersonnelCode)));
        _mockUnitOfWork.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_If_Teacher_Exist_Before() {
        //Arrange
        Create request = new("Teacher_FN_1" , "Teacher_LN_1" , "Teacher_PC_1");

        Teacher expectedTeacher = request.Adapt<Teacher>();
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(request.PersonnelCode))
            .ReturnsAsync(expectedTeacher);

        //Act
        Func<Task<Result>> action = async() => await _handler.Handle(request, default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().Be(String.Format(MessageResults.FoundTeacher , request.PersonnelCode));
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }
}
