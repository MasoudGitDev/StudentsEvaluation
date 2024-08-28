namespace Shared.Files.DTOs;

public record ExamResultDto(ulong StudentId ,
    ulong TeacherId ,
    ulong CourseId ,
    float Score ,
    DateTime ExamDateTime
    ) {
}
