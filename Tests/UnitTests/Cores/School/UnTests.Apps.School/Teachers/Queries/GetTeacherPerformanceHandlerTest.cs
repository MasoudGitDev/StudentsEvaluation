using Apps.School.Domains.Teachers.Queries;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.StudentCourse.Aggregate;
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

        Teacher teacher = Teacher.New("Teacher_FN_1" , "Teacher_LN_1" , request.PersonnelCode);
        teacher.Courses = CreateTeacherCourses(teacher.Id , 3);
        _mockUnitOfWork.Setup(x=> x.Queries.Teachers.GetByPersonnelCodeAsync(teacher.PersonnelCode))
            .ReturnsAsync(teacher);

        //Act
        var result = await _handler.Handle(request,default);

        //Act
        result.Should().NotBeNull().And
            .BeOfType<Result< List<TeacherPerformanceDto>>>();
        _mockUnitOfWork.VerifyAll();
    }





    //==================private
    private static ICollection<Course> CreateTeacherCourses(ulong teacherId , int numberOfCourses = 10 , int numberOfStudents = 10) {
        ICollection<Course> courses = [];
        for(int i = 1 ; i <= numberOfCourses ; i++) {
            var course = Course.New("Course_Code_"+i , "Course_Name_"+i , teacherId);
            course.Students = CreateStudentsCourses(teacherId, numberOfStudents);
            courses.Add(course);
        }
        return courses;
    }
    private static ICollection<StudentCourse> CreateStudentsCourses(ulong teacherId,int studentCourseNumber = 10) {
        ICollection<StudentCourse> studentsCourses = [];
        for(int i = 1 ; i <= studentCourseNumber ; i++) {
            var course = Course.New("Course_Code_"+i, "Course_Name_"+i ,teacherId );
            var student = Student.New("Student_FN_"+i,"Student_LN_"+i , "Student_NC_"+i);
            student.Exams = CreateStudentExamResults(course.Id , teacherId , student.Id);
            StudentCourse studentCourse = new(){
                Student = student ,
                Course = course
            };
            studentsCourses.Add(studentCourse);
        }
        return studentsCourses;
    }

    private static List<ExamResult> CreateStudentExamResults(ulong courseId , ulong teacherId , ulong studentId , int examNumber = 10) {
        List<ExamResult> examResults = [];
        for(int i = 1 ; i <= examNumber ; i++) {
            examResults.Add(ExamResult.New(courseId , teacherId , studentId , DateTime.Today.AddDays(-i) , new Random().Next(14,20)));
        }
        return examResults;
    }
}
