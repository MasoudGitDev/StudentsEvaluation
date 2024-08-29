using Apps.School.Domains.Students.Queries;
using Domains.School.Abstractions;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace UnTests.Apps.School.Students.Queries;
public class GetStudentsMeanScoreHandlerTest {

    private readonly Mock<ISchoolUOW> _mockUnitOfWork = new();
    private readonly GetStudentsMeanScoreHandler _handler;
    public GetStudentsMeanScoreHandlerTest() {
        _handler = new(_mockUnitOfWork.Object);
    }

    [Theory]
    [InlineData(1 , 2)]
    [InlineData(2 , 10)]
    public async Task Handle_Should_Always_ReturnSuccessResult_With_Pagination_With_Descending(
        int pageNumber , int pageSize) {
        //Arrange
        PaginationDto paginationModel = new(true,pageNumber, pageSize);
        GetStudentsMeanScore request = new(paginationModel,true);

        var (students, exams) = CreateStudents();
        var expectedStudents = students.Skip((pageNumber-1) * pageSize).Take(pageSize).ToList();
        _mockUnitOfWork.Setup(x => x.Queries.Students.GetAllAsync(paginationModel))
            .ReturnsAsync(expectedStudents);

        _mockUnitOfWork.Setup(x => x.Queries.Exams.GetStudentExamsAsync(It.IsAny<ulong>()))
            .ReturnsAsync(exams);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<StudentMeanScoreDto>>>();
        result.Model.Should().NotBeNull();
        result.Model?.Count.Should().Be(expectedStudents.Count);
        var firstScore = result.Model!.First().AverageScore;
        var lastScore = result.Model!.Last().AverageScore;
        firstScore.Should().BeGreaterThanOrEqualTo(lastScore); // Descending order
        _mockUnitOfWork.Verify(x => x.Queries.Exams.GetStudentExamsAsync(It.IsAny<ulong>()) ,
            Times.Exactly(expectedStudents.Count));
    }


    [Fact]
    public async Task Handle_Should_Always_ReturnSuccessResult_Without_Pagination_WithDescending() {
        //Arrange
        PaginationDto paginationModel = new(false,0, 0);
        GetStudentsMeanScore request = new(paginationModel,true);

        var (students, exams) = CreateStudents();
        _mockUnitOfWork.Setup(x => x.Queries.Students.GetAllAsync(paginationModel))
            .ReturnsAsync(students);

        _mockUnitOfWork.Setup(x => x.Queries.Exams.GetStudentExamsAsync(It.IsAny<ulong>()))
            .ReturnsAsync(exams);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<StudentMeanScoreDto>>>();
        result.Model.Should().NotBeNull();
        result.Model?.Count.Should().Be(students.Count);
        var firstScore = result.Model!.First().AverageScore;
        var lastScore = result.Model!.Last().AverageScore;
        firstScore.Should().BeGreaterThanOrEqualTo(lastScore); // Descending order
        _mockUnitOfWork.Verify(x => x.Queries.Exams.GetStudentExamsAsync(It.IsAny<ulong>()) ,
            Times.Exactly(students.Count));
    }


    //===========
    private static (List<Student> Students, List<ExamResult> Exams) CreateStudents() {
        List<Student> students = [];
        List<ExamResult> exams = [];
        for(ulong i = 1 ; i <= 11 ; i++) {
            Student student = Student.New(i ,"Student_FN_" + i , "Student_LN_" + i , "Student_NC_" + i);
            student.Exams =
                 CreateExamsForCourse(1 , 1 , student.Id , new Random().Next(0 , 20))
                .Concat(CreateExamsForCourse(2 , 2 , student.Id , new Random().Next(0 , 20)))
                .Concat(CreateExamsForCourse(3 , 3 , student.Id , new Random().Next(0 , 20)))
                .ToList();
            exams.AddRange(student.Exams);
            students.Add(student);
        }
        return (students, exams);
    }
    private static List<ExamResult> CreateExamsForCourse(ulong courseId , ulong teacherId , ulong studentId , float score) {
        List<ExamResult> examResults = [];
        for(int i = 1 ; i <= 10 ; i++) {
            examResults.Add(ExamResult.New(courseId ,
                teacherId ,
                studentId ,
                DateTime.Today.AddDays(-i) ,
                score));
        }
        return examResults;
    }

}
