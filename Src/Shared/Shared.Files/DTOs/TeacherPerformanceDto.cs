namespace Shared.Files.DTOs;
public record TeacherPerformanceDto(
    string CourseCode ,
    string CourseName ,
    int NumberOfStudents ,
    float AverageEducationScore);