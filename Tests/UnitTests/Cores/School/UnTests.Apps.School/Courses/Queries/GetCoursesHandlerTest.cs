using Apps.School.Domains.Courses.Queries;
using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.Shared.Extensions;
using FluentAssertions;
using Moq;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace UnTests.Apps.School.Courses.Queries;
public class GetCoursesHandlerTest {

    private readonly Mock<ISchoolUOW> _mockUnitWork =new();
    private readonly GetCoursesHandler _handler;
    public GetCoursesHandlerTest() {
        _handler = new(_mockUnitWork.Object);
    }

    [Theory]
    [InlineData(1 , 2)]
    [InlineData(2 , 5)]
    public async Task Handle_Should_Always_Return_SuccessResult_With_Pagination(int pageNumber , int pageSize) {
        //Arrange
        PaginationDto paginationDto = new(true,pageNumber,pageSize);
        GetCourses request = GetCourses.New(paginationDto);

        List<Course> courses = [];
        var expectedCourse = (courses.CreateFakeList()).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
        _mockUnitWork.Setup(x => x.Queries.Courses.GetAllAsync(paginationDto))
            .ReturnsAsync(expectedCourse);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<CourseDto>>>();
        result.Model.Should().NotBeNull().And.HaveCount(pageSize);
        _mockUnitWork.VerifyAll();

    }

    [Fact]
    public async Task Handle_Should_Always_Return_SuccessResult_Without_Pagination() {
        //Arrange
        PaginationDto paginationDto = new(false,0,0);
        GetCourses request = GetCourses.New(paginationDto);

        List<Course> courses = [];
        var expectedCourse = courses.CreateFakeList();
        _mockUnitWork.Setup(x => x.Queries.Courses.GetAllAsync(paginationDto))
            .ReturnsAsync(expectedCourse);

        //Act
        var result = await _handler.Handle(request,default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<CourseDto>>>();
        result.Model.Should().NotBeNull().And.HaveCount(expectedCourse.Count);
        _mockUnitWork.VerifyAll();

    }
}
