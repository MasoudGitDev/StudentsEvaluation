using Apps.School.Domains.Students.Queries;
using Domains.School.Abstractions;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Files.Constants;
using Shared.Files.DTOs;
using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace UnTests.Apps.School.Students.Queries;
public class GetStudentMeanScoreHandlerTest {

    private readonly Mock<ISchoolUOW> _unitOfWork;
    private readonly GetStudentMeanScoreHandler _handler;
    public GetStudentMeanScoreHandlerTest() {
        _unitOfWork = new Mock<ISchoolUOW>();
        _handler = new(_unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessResult_With_Expected_AverageScore() {
        //Arrange
        var request = new GetStudentMeanScore("testNationCode");

        Student student = Student.New("Student_FN_1" , "Student_LN_1" , request.NationalCode);
        _unitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(request.NationalCode))
            .ReturnsAsync(student);
        student.Exams = CreateStudentExamResults(student.Id);

        float expectedAveScore = 17.5f;

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().BeOfType<Result<StudentMeanScoreDto>>();
        result.Model?.AverageScore.Should().Be(expectedAveScore);
        _unitOfWork.VerifyAll();
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Student_NoExist_With_NationalCode() {
        //Arrange
        var request = new GetStudentMeanScore("testNationCode");

        _unitOfWork.Setup(x => x.Queries.Students.GetByNationalCodeAsync(request.NationalCode))
            .ReturnsAsync((Student?) null);

        //Act
        Func<Task<Result<StudentMeanScoreDto>>> action = async()=> await _handler.Handle(request, default);

        //Assert
        var exception = await action.Should().ThrowExactlyAsync<CustomException>();
        exception.Which.Description.Should().Be(string.Format(MessageResults.NotFoundStudent , request.NationalCode));
        _unitOfWork.Verify(x => x.Queries.Exams.GetStudentExamsAsync(It.IsAny<ulong>()) , Times.Never);
    }

    //====================
    private static List<ExamResult> CreateStudentExamResults(ulong studentId) {
        List<ExamResult> examResults = [];
        ulong courseId_1 = 1;
        for(int i = 0 ; i < 10 ; i++) {
            examResults.Add(ExamResult.New(
                courseId_1 ,
                1 ,
                studentId ,
                DateTime.UtcNow.AddDays(1) ,
                18
                ));
        }

        ulong courseId_2 = 2;
        for(int i = 0 ; i < 10 ; i++) {
            examResults.Add(ExamResult.New(
                courseId_2 ,
                2 ,
                studentId ,
                DateTime.UtcNow.AddDays(1) ,
                17
                ));
        }
        // average : 17.5
        return examResults;
    }

}
