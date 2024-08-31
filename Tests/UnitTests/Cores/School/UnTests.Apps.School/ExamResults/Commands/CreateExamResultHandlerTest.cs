using Apps.School.Domains.ExamResults.Commands;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Files.Constants;
using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace UnTests.Apps.School.ExamResults.Commands;
public class CreateExamResultHandlerTest {

    private readonly Mock<ISchoolUOW> _mockUnitOfWork = new();
    private readonly CreateExamResultHandler _handler;
    public CreateExamResultHandlerTest() {
        _handler = new(_mockUnitOfWork.Object);
    }

    private readonly DateTime _lessTheNowDateTime = DateTime.Today.AddDays(-1);

    [Fact]
    public async Task Handle_Should_Create_ExamResult_Successfully() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , 18.5f , _lessTheNowDateTime);

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
            .ReturnsAsync(teacher);

        Course course = Course.New(request.CourseCode , "Course_Name_1" , teacher.Id);
        _mockUnitOfWork.Setup(x => x.Queries.Courses.GetByCodeAsync(course.Code))
            .ReturnsAsync(course);

        Student student = Student.New("Student_FN_1", "Student_LN_1" , "Student_NC_1");
        _mockUnitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(student.NationalCode,LoadingType.Lazy))
            .ReturnsAsync(student);

        _mockUnitOfWork.Setup(x => x.Queries.Exams.HadStudentAnyExamAsync(student.Id , course.Id , request.ExamDateTime))
            .ReturnsAsync((ExamResult?) null);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should()
            .NotBeNull().And
            .BeEquivalentTo(Result.Success(String.Format(MessageResults.CreateExamResult , string.Empty)));
        request.Score.Should().BeInRange(0 , 20);
        request.ExamDateTime.Should().BeBefore(DateTime.UtcNow);
        _mockUnitOfWork.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_ExamDateTime_Is_Invalid() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , 18 , DateTime.UtcNow.AddMinutes(1));

        //Act
        var action = async () => await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should()
            .NotBeNull().And
            .BeEquivalentTo(MessageResults.DateTimeError);

        _mockUnitOfWork.Verify(x => x.Queries.Teachers.GetByPersonnelCodeAsync(request.TeacherPersonnelCode) , Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Score_Is_InRange_0_20() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , -5f , _lessTheNowDateTime);

        //Act
        var action = async () => await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should()
            .NotBeNull().And
            .BeEquivalentTo(String.Format(MessageResults.InvalidScore , request.Score));

        _mockUnitOfWork.Verify(x => x.Queries.Teachers.GetByPersonnelCodeAsync(request.TeacherPersonnelCode) , Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Teacher_NotFound() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , 18.5f , DateTime.UtcNow);

        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(request.TeacherPersonnelCode))
            .ReturnsAsync((Teacher?) null);

        //Act
        var action = async() => await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().BeEquivalentTo(
            String.Format(MessageResults.NotFoundTeacher , request.TeacherPersonnelCode));
        _mockUnitOfWork.Verify(x => x.Queries.Courses.GetByCodeAsync(It.IsAny<string>()) , Times.Never);
        _mockUnitOfWork.Verify(x => x.Queries.Students.GetByNationalCodeAsync(It.IsAny<string>(),LoadingType.Lazy) , Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Course_NotFound() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , 18.5f , _lessTheNowDateTime);

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
            .ReturnsAsync(teacher);

        _mockUnitOfWork.Setup(x => x.Queries.Courses.GetByCodeAsync(request.CourseCode))
            .ReturnsAsync((Course?) null);

        //Act
        var action = async() => await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().BeEquivalentTo(
            String.Format(MessageResults.NotFoundCourse , request.CourseCode));

        _mockUnitOfWork.Verify(x => x.Queries.Students.GetByNationalCodeAsync(It.IsAny<string>(), LoadingType.Lazy) , Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Student_NotFound() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , 18.5f , _lessTheNowDateTime);

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
            .ReturnsAsync(teacher);

        Course course = Course.New(request.CourseCode , "Course_Name_1" , teacher.Id);
        _mockUnitOfWork.Setup(x => x.Queries.Courses.GetByCodeAsync(course.Code))
            .ReturnsAsync(course);

        _mockUnitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(request.StudentNationalCode,LoadingType.Lazy))
            .ReturnsAsync((Student?) null);

        //Act
        var action = async() => await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().BeEquivalentTo(
            String.Format(MessageResults.NotFoundStudent , request.StudentNationalCode));
        _mockUnitOfWork.Verify(x => x.Queries.Exams.HadStudentAnyExamAsync(It.IsAny<ulong>() ,
            course.Id ,
            request.ExamDateTime
        ) , Times.Never);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_ExamResult_IsNotFollow_PerCourse_PerDay_For_Each_Student() {
        //Arrange
        Create request = new("Teacher_PC_1" , "Course_Code_1" , "Student_NC_1" , 18.5f , _lessTheNowDateTime);

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.TeacherPersonnelCode);
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
            .ReturnsAsync(teacher);

        Course course = Course.New(request.CourseCode , "Course_Name_1" , teacher.Id);
        _mockUnitOfWork.Setup(x => x.Queries.Courses.GetByCodeAsync(course.Code))
            .ReturnsAsync(course);

        Student student = Student.New("Student_FN_1", "Student_LN_1" , "Student_NC_1");
        _mockUnitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(student.NationalCode, LoadingType.Lazy))
            .ReturnsAsync(student);

        ExamResult examResult = ExamResult.New(course.Id,teacher.Id,student.Id,_lessTheNowDateTime, 18);
        _mockUnitOfWork.Setup(x => x.Queries.Exams.HadStudentAnyExamAsync(student.Id , course.Id , _lessTheNowDateTime))
            .ReturnsAsync(examResult);

        //Act
        var action = async() => await _handler.Handle(request,default);

        //Assert
        var exception = await Assert.ThrowsAsync<CustomException>(action);
        exception.Description.Should().BeEquivalentTo(
            String.Format(MessageResults.OneExamPerCoursePerDay , string.Empty));

        _mockUnitOfWork.Verify(x => x.SaveChangesAsync() , Times.Never);
    }

}
