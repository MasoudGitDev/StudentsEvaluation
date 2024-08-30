namespace Shared.Files.DTOs;

public record ExamResultDto(string TeacherPersonnelCode ,
    string CourseCode ,
    string StudentNationalCode ,
    float Score ,
    DateTime ExamDateTime
 );