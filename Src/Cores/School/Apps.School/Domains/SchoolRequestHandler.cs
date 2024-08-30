﻿using Domains.School.Abstractions;
using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Mapster;
using MediatR;
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

    // Queries
    protected async Task<List<TeacherDto>> GetTeacherDTOsAsync(PaginationDto model)
        => ( await _unitOfWork.Queries.Teachers.GetAllAsync(model)).Adapt<List<TeacherDto>>();

    protected async Task<List<StudentDto>> GetStudentDTOsAsync(PaginationDto model)
        => (await _unitOfWork.Queries.Students.GetAllAsync(model)).Adapt<List<StudentDto>>();

    protected async Task<List<ExamResult>> GetStudentExamsAsync(ulong studentId)
        => await _unitOfWork.Queries.Exams.GetStudentExamsAsync(studentId);


    protected async Task<Student?> FindStudentByCodeAsync(string nationalCode)
        => await _unitOfWork.Queries.Students.GetByNationalCodeAsync(nationalCode);

    protected async Task<List<CourseDto>> GetCoursesAsync(PaginationDto model)
        => (await _unitOfWork.Queries.Courses.GetAllAsync(model)).Adapt<List<CourseDto>>();

    protected async Task<Course?> FindCourseByCodeAsync(string code)
        => await _unitOfWork.Queries.Courses.GetByCodeAsync(code);

    protected async Task<Teacher?> FindTeacherByCodeAsync(string personnelCode)
        => await _unitOfWork.Queries.Teachers.GetByPersonnelCodeAsync(personnelCode);

    protected async Task<ExamResult?> HadStudentAnyExamAsync(ulong studentId , ulong courseId , DateTime examDateTime)
        => await _unitOfWork.Queries.Exams.HadStudentAnyExamAsync(studentId , courseId , examDateTime);

    // Results
    protected static Result<List<T>> SuccessListResult<T>(string propertyName , List<T> items) {
        string message = items.Count > 0
            ? $"{items.Count} {propertyName} found."
            : $"There is no any {propertyName} !";
        return Result<List<T>>.Success(message , items);
    }
}
