using Apps.School.Domains.Courses.Commands;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Mapster;
using Moq;
using Shared.Files.Constants;
using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace UnTests.Apps.School.Courses.Commands;
public class CreateCourseHandlerTest {
    private readonly Mock<ISchoolUOW> _unitOfWork = new();
    private readonly CreateCourseHandler _handler;
    public CreateCourseHandlerTest() {
        _handler = new(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Successfully_When_Course_NotExist_Before() {
        //Arrange
        Create request = new("Course_Code_1" , "Course_Name_1" , "Teacher_PC_1");

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _unitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
           .ReturnsAsync(teacher);

        _unitOfWork.Setup(x => x.Queries.Courses.GetByCodeAsync(request.Code))
            .ReturnsAsync((Course?) null);

        Course course = request.Adapt<Course>();
        _unitOfWork.Setup(x => x.CreateAsync(It.IsAny<Course>()))
            .Callback<Course>(courseItem => courseItem = course);

        _unitOfWork.Setup(x => x.SaveChangesAsync());

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().BeEquivalentTo(Result.Success(String.Format(MessageResults.CreateCourse , request.Code)));
        _unitOfWork.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Course_Exist_Before() {
        //Arrange
        Create request = new("Course_Code_1" , "Course_Name_1" , "Teacher_PC_1");

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _unitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
           .ReturnsAsync(teacher);

        Course course = request.Adapt<Course>();
        _unitOfWork.Setup(x => x.Queries.Courses.GetByCodeAsync(request.Code))
            .ReturnsAsync(course);

        //Act
        var action = async()=> await _handler.Handle(request,default);

        //Assert
        var exception  = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().Be(String.Format(MessageResults.FoundCourse , request.Code));
        _unitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Teacher_NotExist() {
        //Arrange
        Create request = new("Course_Code_1" , "Course_Name_1" , "Teacher_PC_1");

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _unitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
           .ReturnsAsync((Teacher?) null);

        //Act
        var action = async()=> await _handler.Handle(request,default);

        //Assert
        var exception  = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().Be(String.Format(MessageResults.NotFoundTeacher , request.TeacherPersonnelCode));
        _unitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }
}
