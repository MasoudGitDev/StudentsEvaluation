using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Shared.Files.Models;

namespace Infra.SqlServerWithEF.DummyData;

internal class ProjectDummyData(ISchoolUOW _unitOfWork) {


    public async Task<Result> ExecuteAsync() {
        try {
            var students = await GetOrCreateStudentsAsync();
            var teachers = await GetOrCreateTeachersAsync();
            //=========
          // var teachersCourses = await CreateTeachersCoursesAsync(teachers);
            //await CreateAllExamResultsAsync(students, teachersCourses);

            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Ok");
        }
        catch(Exception ex) {
            return Result.Failed(ex.Message);
        }
    }


    private async Task<List<Teacher>> GetOrCreateTeachersAsync() {
        return await IsAnyTeachersAsync() ?
            await _unitOfWork.Queries.Teachers.GetAllAsync(new(false)) :
            await CreateTeachersAsync(10);
    }
    private async Task<List<Student>> GetOrCreateStudentsAsync() {
        return await IsAnyStudentsAsync() ?
            await _unitOfWork.Queries.Students.GetAllAsync(new(false)) :
            await CreateStudentsAsync(40);
    }


    //==============================
    private async Task<List<TModel>> CreateListAsync<TModel>(int count , Func<int , TModel> action)
        where TModel : class, new() {
        List<TModel> items = [];
        for(int i = 1 ; i <= count ; i++) {
            var model = action.Invoke(i);
            items.Add(model);
            await _unitOfWork.CreateAsync(model);
        }
        return items;
    }

    private async Task<List<Student>> CreateStudentsAsync(int count = 40)
       => await CreateListAsync(count ,
           (i) => Student.New("Student_FName_" + i , "Student_LName_" + i , "Student_NCode_" + i));

    private async Task<List<Teacher>> CreateTeachersAsync(int count = 10)
          => await CreateListAsync(count ,
              (i) => Teacher.New("Teacher_FName_" + i , "Teacher_LName_" + i , "Teacher_PCode_" + i));

    private async Task<List<Course>> CreateTeachersCoursesAsync(List<Teacher> teachers) { 
        List<Course> allTeachersCourses = [];
        ulong mainCourseCounter = 1 ;
        foreach(var teacher in teachers) {
            teacher.Courses = await CreateTeacherCourses(teacher , mainCourseCounter , 3);
            mainCourseCounter += 3;
        }
        return allTeachersCourses;
    }

    private Task<List<Course>> CreateTeacherCourses(Teacher teacher, ulong startCourseId , ulong courseCount = 3) {
        List<Course> teacherCourses = [];
        for(ulong i = startCourseId ; i <= courseCount ; i++) {
            Course course = Course.New("Course_Code_"+i , "Course_Name_"+i , teacher.Id);
            teacher.Courses.Add(course);
        }
        return Task.FromResult(teacherCourses);
    }

    private async Task CreateAllExamResultsAsync(List<Student> students,List<Course> teachersCourses) {
        foreach(var student in students) {
            await CreateStudentExamResultsAsync(student , teachersCourses);
        }
    }

    private async Task<Student> CreateStudentExamResultsAsync(Student student , List<Course> teachersCourses) {
        foreach(var course in teachersCourses) {
            student.Exams = await CreateExamResultAsync(10 , course.Id , course.Teacher.Id , student.Id , DateTime.Today.AddDays(-1) , 18);
        }
        return student;
    }

    private async Task<List<ExamResult>> CreateExamResultAsync(int count ,
        ulong courseId , 
        ulong teacherId ,
        ulong studentId , 
        DateTime examDateTime , 
        float score)
        => await CreateListAsync(count ,
              (i) => ExamResult.New(courseId, teacherId,studentId,examDateTime,score));



    private async Task<List<Course>> CreateCoursesAsync( ulong teacherId, int count = 40)
          => await CreateListAsync(count ,
              (i) => Course.New((ulong) i,"Course_Code_" + i , "Course_Name_" + i , teacherId));


    private async Task<bool> IsAnyStudentsAsync()
        => ( await _unitOfWork.Queries.Students.GetAllAsync(new(false)) ).Count > 0;

    private async Task<bool> IsAnyTeachersAsync()
       => ( await _unitOfWork.Queries.Teachers.GetAllAsync(new(false)) ).Count > 0;

}
