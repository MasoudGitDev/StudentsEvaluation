using Domains.School.Course.Aggregate;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Infra.SqlServerWithEF.Contexts;
using Infra.SqlServerWithEF.Impls.School;
using Microsoft.EntityFrameworkCore;
using Shared.Files.DTOs;

namespace UNTests.Infra.SqlServerWithEF.Impls.School;
public class CourseQueriesTests : IAsyncDisposable {

    private readonly AppDbContext _appDbContext;
    private readonly CourseQueries _handler ;

    public CourseQueriesTests() {

        var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "CourseDbTest");
        _appDbContext = new(dbContextOptionBuilder.Options);

        CreateDummyData(10).GetAwaiter().GetResult();

        _appDbContext.SaveChanges();
        _handler = new(_appDbContext);

    }

    //=========================================Get All Courses Tests
    [Theory]
    [InlineData(2 , 5)]
    [InlineData(1 , 10)]
    public async Task GetAll_Should_Return_CoursesList_With_Pagination(int pageNumber , int pageSize) {
        // Arrange
        PaginationDto paginationModel = new(true, pageNumber, pageSize);

        // must eager load with Teacher
        var courses = _appDbContext.Courses.Include(x=>x.Teacher).AsQueryable();

        var expectedResult = courses
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Act
        var result = await _handler.GetAllAsync(paginationModel);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetAll_Should_Return_CoursesList_Without_Pagination() {
        // Arrange
        PaginationDto paginationModel = new(false, 0,0);

        // must eager load with Teacher
        var courses = _appDbContext.Courses.Include(x=>x.Teacher).AsQueryable();
        var expectedCourses = courses
            .ToList();

        // Act
        var result = await _handler.GetAllAsync(paginationModel);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedCourses);
    }


    //========================================== Get Course By Code
    [Fact]
    public async Task GetByCode_Should_Return_Special_Course() {
        //Arrange
        string courseCode = "Course_Code_1";
        var expectedCourse = await _appDbContext.Courses.FirstOrDefaultAsync(x=>x.Code == courseCode);

        //Act
        var result = await _handler.GetByCodeAsync(courseCode);

        //Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedCourse);
    }

    //========================================== Get Course By ID
    [Fact]
    public async Task GetById_Should_Return_Special_Course() {
        //Arrange
        ulong courseId = 1;
        var expectedCourse = await _appDbContext.Courses.FirstOrDefaultAsync(x=>x.Id == courseId);

        //Act
        var result = await _handler.GetByIdAsync(courseId);

        //Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedCourse);
    }





    //================== private
    private async Task CreateDummyData(ulong count = 10) {
        ulong startingValue = (ulong)_appDbContext.Courses.Count(); // Ensures unique ID in each run
        for(ulong i = startingValue + 1 ; i <= count ; i++) {
            Teacher teacher = Teacher.New("Teacher_FN_"+i , "Teacher_LN_"+i , "Teacher_PCode_"+i);
            Course course = Course.New(i, "Course_Code_" + i, "Course_Name_" + i, teacher);
            teacher.Courses.Add(course);
            await _appDbContext.AddAsync(course);
            await _appDbContext.AddAsync(teacher);
        }
    }


    public async ValueTask DisposeAsync() {
        _appDbContext.Courses.RemoveRange(_appDbContext.Courses);
        await _appDbContext.SaveChangesAsync();
       await _appDbContext.Database.EnsureDeletedAsync();
    }
}