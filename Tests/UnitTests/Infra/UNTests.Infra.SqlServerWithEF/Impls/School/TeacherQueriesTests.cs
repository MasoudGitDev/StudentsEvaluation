using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Teacher.Aggregate;
using FluentAssertions;
using Infra.SqlServerWithEF.Contexts;
using Infra.SqlServerWithEF.Impls.School;
using Microsoft.EntityFrameworkCore;
using Shared.Files.DTOs;

namespace UNTests.Infra.SqlServerWithEF.Impls.School;
public class TeacherQueriesTests : IAsyncDisposable {

    private readonly AppDbContext _appDbContext;
    private readonly TeacherQueries _handler ;

    public TeacherQueriesTests() {

        var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "TeacherDbTest");
        _appDbContext = new(dbContextOptionBuilder.Options);

        CreateDummyData(10).GetAwaiter().GetResult();

        _appDbContext.SaveChanges();
        _handler = new(_appDbContext);

    }

    //=========================================Get All Courses Tests
    [Theory]
    [InlineData(2 , 5)]
    [InlineData(1 , 10)]
    public async Task GetAll_Should_Return_TeacherList_With_Pagination(int pageNumber , int pageSize) {
        // Arrange
        PaginationDto paginationModel = new(true, pageNumber, pageSize);

        var items = _appDbContext.Teachers.AsQueryable();

        var expectedResult = items
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Act
        var result = await _handler.GetAllAsync(paginationModel);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetAll_Should_Return_TeacherList_Without_Pagination() {
        // Arrange
        PaginationDto paginationModel = new(false, 0,0);

        // must eager load
        var courses = _appDbContext.Teachers.AsQueryable();
        var expectedCourses = courses
            .ToList();

        // Act
        var result = await _handler.GetAllAsync(paginationModel);

        // Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedCourses);
    }


    //========================================== Get By Code
    [Fact]
    public async Task GetByCode_Should_Return_Special_Teacher() {
        //Arrange
        string code = "Teacher_PCode_1";
        var expectedCourse = await _appDbContext.Teachers
            .Include(x=>x.Courses)
            .ThenInclude(course => course.Exams)
            .FirstOrDefaultAsync(x=>x.PersonnelCode == code);

        //Act
        var result = await _handler.GetByPersonnelCodeAsync(code);

        //Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedCourse);
        result?.Courses.Should().NotBeNull();
        // Check that each course has the expected number of exams
        foreach(var course in result!.Courses!) {
            course.Exams.Should().NotBeNull();
            course.Exams.Should().NotBeEmpty(); // Ensure there are exams
            course.Exams.Count.Should().Be(10); // Assuming you expect each course to have 10 exams
        }

    }

    //========================================== Get By ID
    [Fact]
    public async Task GetById_Should_Return_Special_Teacher() {
        //Arrange
        ulong id = 1;
        var expectedItem = await _appDbContext.Teachers.FirstOrDefaultAsync(x=>x.Id == id);

        //Act
        var result = await _handler.GetByIdAsync(id);

        //Assert
        result.Should().NotBeNull().And.BeEquivalentTo(expectedItem);
    }





    //================== private
    private async Task CreateDummyData(ulong count = 10) {
        ulong startingValue = (ulong)_appDbContext.Teachers.Count(); // Ensures unique ID in each run
        for(ulong i = startingValue + 1 ; i <= count ; i++) {
            Teacher teacher = Teacher.New("Teacher_FN_"+i , "Teacher_LN_"+i , "Teacher_PCode_"+i);
            Course course = Course.New(i, "Course_Code_" + i, "Course_Name_" + i, teacher);
            course.Exams = CreateExams(10 , course.Id , teacher.Id);
            teacher.Courses.Add(course);
            await _appDbContext.AddAsync(course);
            await _appDbContext.AddAsync(teacher);
        }
    }

    private List<ExamResult> CreateExams(ulong examCount , ulong courseId , ulong teacherId) {
        List<ExamResult> exams = [];
        for(ulong i = 1 ; i <= examCount ; i++) {
            exams.Add(ExamResult.New(courseId , teacherId , i , DateTime.Today.AddDays(-(int) i) , 18));
        }
        return exams;
    }


    public async ValueTask DisposeAsync() {
        _appDbContext.Teachers.RemoveRange(_appDbContext.Teachers);
        await _appDbContext.SaveChangesAsync();
        await _appDbContext.Database.EnsureDeletedAsync();
    }
}