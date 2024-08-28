using Apps.School.Domains.Students.Queries;
using Domains.School.Abstractions;
using Domains.School.Student.Aggregate;
using FluentAssertions;
using Mapster;
using Moq;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace UnTests.Apps.School.Students.Queries;
public class GetStudentsHandlerTest {

    private readonly Mock<ISchoolUOW> _unitOfWork;
    private readonly GetStudentsHandler _handler;

    public GetStudentsHandlerTest() {
        _unitOfWork = new Mock<ISchoolUOW>();
        _handler = new(_unitOfWork.Object);
    }


    [Theory]
    [InlineData(1 , 30)]
    [InlineData(2 , 20)]
    public async Task Handle_Should_always_Return_SuccessResult_With_Use_Pagination(int pageNumber , int pageSize) {
        //Arrange
        GetStudents request = GetStudents.New(true, pageNumber, pageSize);

        List<Student> students = CreateFakeStudents() ;
        List<Student> expectedStudents = students
            .Skip((pageNumber-1)*pageNumber).Take(pageSize).ToList();

        _unitOfWork.Setup(x => x.Queries.Students.GetAllAsync(true , pageNumber , pageSize))
            .ReturnsAsync(expectedStudents);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<StudentDto>>>();
        result?.Model?.Count.Should().Be(pageSize);
        _unitOfWork.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_always_Return_SuccessResult_Without_Use_Pagination() {
        //Arrange
        GetStudents request = GetStudents.New(false, 1, 50);

        List<Student> students = CreateFakeStudents() ;
        _unitOfWork.Setup(x => x.Queries.Students.GetAllAsync(request.UsePagination , request.PageNumber , request.PageSize))
            .ReturnsAsync(students);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<StudentDto>>>();
        result?.Model?.Count.Should().Be(students.Count);
        _unitOfWork.VerifyAll();
    }

    //======================
    private static List<Student> CreateFakeStudents() {
        List<Student> students = [];
        for(int i = 1 ; i <= 100 ; i++) {
            students.Add(Student.New("Student_FN_" + i , "Student_LN_" + i , "Student_NC_" + i));
        }
        return students;
    }
}
