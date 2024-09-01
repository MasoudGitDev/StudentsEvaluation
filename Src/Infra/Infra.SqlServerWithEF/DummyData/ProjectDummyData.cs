using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Infra.SqlServerWithEF.Contexts;
using Shared.Files.Models;

namespace Infra.SqlServerWithEF.DummyData;

internal class ProjectDummyData(ISchoolUOW _unitOfWork , AppDbContext _dbContext) {


    public async Task<Result> ExecuteAsync() {
        try {
            var students = await GetOrCreateStudentsAsync(10);
            var teachers = await GetOrCreateTeachersAsync(3);
            //=========
            var courses = await GetOrCreateCoursesAsync(teachers);
            var allExams = CreateExams(students , courses);
            await CreateAllExamResultsIfNeededAsync(allExams);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Ok");
        }
        catch(Exception ex) {
            return Result.Failed(ex.Message);
        }
    }


    private async Task<List<Teacher>> GetOrCreateTeachersAsync(ulong teachersNumber = 3) {
        return await IsAnyTeachersAsync() ?
            await _unitOfWork.Queries.Teachers.GetAllAsync(new(false)) :
            await CreateTeachersAsync(teachersNumber);
    }
    private async Task<List<Student>> GetOrCreateStudentsAsync(ulong studentsNumber = 10) {
        return await IsAnyStudentsAsync() ?
            await _unitOfWork.Queries.Students.GetAllAsync(new(false)) :
            await CreateStudentsAsync(studentsNumber);
    }

    private async Task<List<Course>> GetOrCreateCoursesAsync(List<Teacher> teachers) {
        var findCourses = await _unitOfWork.Queries.Courses.GetAllAsync(new(false));
        if(findCourses.Count != 0) {
            return findCourses;
        }
        return await CreateTeachersCoursesAsync(teachers);
    }



    //==============================
    private async Task<List<TModel>> CreateListAsync<TModel>(ulong count , Func<ulong , TModel> action)
        where TModel : class, new() {
        List<TModel> items = [];
        for(ulong i = 1 ; i <= count ; i++) {
            var model = action.Invoke(i);
            items.Add(model);
            await _unitOfWork.CreateAsync(model);
        }
        return items;
    }

    private async Task<List<Student>> CreateStudentsAsync(ulong count)
       => await CreateListAsync(count ,
           (id) => Student.New($"Student_FName_{id}" , $"Student_LName_{id}" , $"Student_NCode_{id}"));

    private async Task<List<Teacher>> CreateTeachersAsync(ulong count)
          => await CreateListAsync(count ,
              (id) => Teacher.New($"Teacher_FName_{id}" , $"Teacher_LName_{id}" , $"Teacher_PCode_{id}"));

    private async Task<List<Course>> CreateTeachersCoursesAsync(List<Teacher> teachers) {
        List<Course> allTeachersCourses = [];
        ulong mainCourseCounter = 1 ;
        foreach(var teacher in teachers) {
            ulong courseCount = 2;
            var courses = await CreateTeacherCourses(teacher , mainCourseCounter , courseCount);
            teacher.Courses = courses;
            mainCourseCounter += ( courseCount + 1 );
            allTeachersCourses.AddRange(courses);
        }
        return allTeachersCourses;
    }

    private Task<List<Course>> CreateTeacherCourses(Teacher teacher , ulong startCourseId , ulong courseCount = 3) {
        List<Course> teacherCourses = [];
        for(ulong i = startCourseId ; i <= courseCount + startCourseId ; i++) {
            Course course = Course.New("Course_Code_"+i , "Course_Name_"+i , teacher.Id);
            teacher.Courses.Add(course);
            teacherCourses.Add(course);
        }
        return Task.FromResult(teacherCourses);
    }


    private async Task CreateAllExamResultsIfNeededAsync(List<ExamResult> allExams) {
        try {
            if(_dbContext.ExamResults.Any()) {
                return;
            }
            await _dbContext.ExamResults.AddRangeAsync(allExams);
        }
        catch(Exception ex) {
            throw;
        }
    }

    private async Task<bool> IsAnyStudentsAsync()
        => ( await _unitOfWork.Queries.Students.GetAllAsync(new(false)) ).Count > 0;

    private async Task<bool> IsAnyTeachersAsync()
       => ( await _unitOfWork.Queries.Teachers.GetAllAsync(new(false)) ).Count > 0;

    private List<ExamResult> CreateExams(List<Student> students , List<Course> courses) {
        ulong i = 1;
        List<ExamResult> exams = [];
        foreach(var course in courses) {
            foreach(var student in students) {
                var exam = ExamResult.New(course,student,DateTime.Today.AddDays(-(int)i),
                    new Random().Next(10,20));
                exams.Add(exam);
                student.Exams = [exam];
                course.Exams = [exam];
                i++;
            }
        }
        return ([.. exams.OrderBy(x => x.StudentId)]);
    }

}
