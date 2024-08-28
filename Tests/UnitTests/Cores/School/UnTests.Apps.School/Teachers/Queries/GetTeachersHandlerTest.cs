using Apps.School.Domains.Teachers.Queries;
using Domains.School.Abstractions;
using Domains.School.Shared.Extensions;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Moq;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace UnTests.Apps.School.Teachers.Queries;
public class GetTeachersHandlerTest {
    private readonly Mock<ISchoolUOW> _mockUnitOfWork = new();
    private readonly GetTeachersHandler _handler;
    public GetTeachersHandlerTest() {
        _handler = new(_mockUnitOfWork.Object);
    }

    [Theory]
    [InlineData(1 , 2)]
    [InlineData(2 , 10)]
    public async Task Handle_Should_Always_Return_SuccessResult_With_Pagination(int pageNumber , int pageSize) {
        //Arrange 
        PaginationDto paginationDto = new(true,pageNumber,pageSize);
        GetTeachers request = new(paginationDto);
        List<Teacher> teachers = [] ;
        teachers = teachers.CreateFakeList();
        var expectedTeachers = teachers.Skip((pageNumber-1) * pageSize).Take(pageSize)
            .ToList();
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetAllAsync(paginationDto))
            .ReturnsAsync(expectedTeachers);



        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<TeacherDto>>>();
        result?.Model?.Should().NotBeNull().And.HaveCount(pageSize);
        _mockUnitOfWork.VerifyAll();

    }

    [Fact]
    public async Task Handle_Should_Always_Return_SuccessResult_Without_Pagination() {
        //Arrange 
        PaginationDto paginationDto = new(true,0,0);
        GetTeachers request = new(paginationDto);
        List<Teacher> expectedTeachers = [] ;
        expectedTeachers = expectedTeachers.CreateFakeList();
        _mockUnitOfWork.Setup(x => x.Queries.Teachers.GetAllAsync(paginationDto))
            .ReturnsAsync(expectedTeachers);



        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        result.Should().NotBeNull().And.BeOfType<Result<List<TeacherDto>>>();
        result?.Model?.Should().NotBeNull().And.HaveCount(expectedTeachers.Count);
        _mockUnitOfWork.VerifyAll();

    }
}
