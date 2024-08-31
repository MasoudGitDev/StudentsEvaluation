using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Mapster;
using MediatR;
using Shared.Files.Constants;
using Shared.Files.DTOs;
using Shared.Files.Models;

namespace Apps.School.Domains;
internal abstract class SchoolRequestHandler<TRequest, TResult>(ISchoolUOW _unitOfWork) :
    IRequestHandler<TRequest , TResult>
        where TRequest : IRequest<TResult>
        where TResult : class, new() {

    public abstract Task<TResult> Handle(TRequest request , CancellationToken cancellationToken);

    //Commands
    protected async Task<Result> CreateAndSaveAsync<TEntity>(TEntity entity , string messageFormat , params string[] names)
      where TEntity : class, new() {
        try {
            await _unitOfWork.CreateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success(String.Format(messageFormat , names));
        }
        catch(Exception ex) {
            return Result.Failed(ex.Message);
        }
    }

    protected Result<StudentMeanScoreDto> CalculateStudentAverageScore(Student student) {
        var studentExamResults = student.Exams.Where(examResult=> examResult.StudentId == student.Id);
        if(studentExamResults is null || student.Exams.Count <= 0) {
            return Result<StudentMeanScoreDto>
                .Failed($"Not found any exam for Student with national-code : <{student.NationalCode}> ");
        }
        float averageScore = studentExamResults.Average(x => x.Score);
        return Result<StudentMeanScoreDto>.Success("OK" , new(
            student.FirstName ,
            student.LastName ,
            student.NationalCode ,
            averageScore));
    }

    // Queries
    protected async Task<List<TeacherDto>> GetTeacherDTOsAsync(PaginationDto model)
        => ( await _unitOfWork.Queries.Teachers.GetAllAsync(model)).Adapt<List<TeacherDto>>();

    protected async Task<List<StudentDto>> GetStudentDTOsAsync(PaginationDto model,LoadingType loadingType = LoadingType.Lazy)
        => (await _unitOfWork.Queries.Students.GetAllAsync(model,loadingType)).Adapt<List<StudentDto>>();

    protected async Task<Student?> FindStudentByCodeAsync(string nationalCode , LoadingType loadingType = LoadingType.Lazy)
        => await _unitOfWork.Queries.Students.GetByNationalCodeAsync(nationalCode ,loadingType);

    protected async Task<List<CourseDto>> GetCoursesAsync(PaginationDto model)
        => (await _unitOfWork.Queries.Courses.GetAllAsync(model)).Adapt<List<CourseDto>>();

    protected async Task<Course?> FindCourseByCodeAsync(string code)
        => await _unitOfWork.Queries.Courses.GetByCodeAsync(code);

    protected async Task<Teacher?> FindTeacherByCodeAsync(string personnelCode)
        => await _unitOfWork.Queries.Teachers.GetByPersonnelCodeAsync(personnelCode);

    // Results
    protected static Result<List<T>> SuccessListResult<T>(string propertyName , List<T> items) {
       return items.Count > 0 ? 
              Result<List<T>>.Success($"{items.Count} {propertyName} found." , items) :
              Result<List<T>>.Warning($"There is no any {propertyName} !" , items);
        
    }
}
