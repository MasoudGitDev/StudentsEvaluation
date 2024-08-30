using Apps.School.Domains.Teachers.Queries;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace UnTests.Apps.School.Teachers.Queries;
public class GetTeacherPerformanceHandlerTest {

    private readonly Mock<ISchoolUOW> _mockUnitOfWork = new();
    private readonly GetTeacherPerformanceHandler _handler;

    public GetTeacherPerformanceHandlerTest() {
        _handler = new(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_SuccessListResult_When_Teacher_Exist() {
        //Arrange
        GetTeacherPerformance request = new("Teacher_PC_1");

        Teacher teacher = Teacher.New(1,"Teacher_FN_1" , "Teacher_LN_1" , request.PersonnelCode);
        teacher.Courses = CreateTeacherCourses(teacher ,
            numberOfCourses: 1 ,
            numberOfStudents : 3 ,
            numberOfExams : 1);

        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
            .ReturnsAsync(teacher);

        //Act
        var result = await _handler.Handle(request,default);

        //Act
        result.Should().NotBeNull().And
            .BeOfType<Result<List<TeacherPerformanceDto>>>();
        _mockUnitOfWork.VerifyAll();
    }





    //==================private
    private static ICollection<Course> CreateTeacherCourses(Teacher teacher,
        int numberOfCourses = 3 ,
        int numberOfStudents = 10 , 
        int numberOfExams = 10) {
        ICollection<Course> courses = [];
        for(int i = 1 ; i <= numberOfCourses ; i++) {
            var course = Course.New((ulong)i ,"Course_Code_"+i , "Course_Name_"+i , teacher.Id);
            course.Exams = CreateExamResults(teacher.Id , course , numberOfStudents , numberOfExams);
            courses.Add(course);
        }
        return courses;
    }


    private static ICollection<ExamResult> CreateExamResults(ulong teacherId ,
        Course course ,
        int studentNumber = 10 ,
        int examNumber = 10) {
        List<ExamResult> studentsExams = [];
        for(int i = 1 ; i <= studentNumber ; i++) {
            var student = Student.New((ulong)i,"Student_FN_"+i,"Student_LN_"+i , "Student_NC_"+i);
            var studentExams = CreateStudentExamResults(course , teacherId , student.Id,examNumber);
            student.Exams = studentExams;
            studentsExams.AddRange(studentExams);
        }
        return studentsExams;
    }

    private static List<ExamResult> CreateStudentExamResults(Course course , ulong teacherId , ulong studentId , int examNumber = 10) {
        List<ExamResult> examResults = [];
        for(int i = 1 ; i <= examNumber ; i++) {
            examResults.Add(ExamResult.New((ulong)i ,
                course , 
                teacherId , 
                studentId , 
                DateTime.Today.AddDays(-i) , 
                new Random().Next(14 , 20)));
        }
        return examResults;
    }
}
