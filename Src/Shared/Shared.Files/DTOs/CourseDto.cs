namespace Shared.Files.DTOs;

public record CourseDto(string Code , string Name, string TeacherPersonnelCode) {
    public static CourseDto New(string code , string name , string teacherPersonnelCode)
        => new(code,name,teacherPersonnelCode);
}
