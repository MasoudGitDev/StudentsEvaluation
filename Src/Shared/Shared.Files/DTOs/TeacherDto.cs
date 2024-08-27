namespace Shared.Files.DTOs;

public record TeacherDto(string FirstName , string LastName , string PersonnelCode) {
    public static TeacherDto New(string firstName , string lastName , string personnelCode)
        => new(firstName , lastName , personnelCode);
}