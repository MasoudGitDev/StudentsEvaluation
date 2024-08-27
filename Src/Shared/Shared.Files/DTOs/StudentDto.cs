namespace Shared.Files.DTOs;
public record StudentDto(string FirstName , string LastName , string NationalCode) {
    public static StudentDto New(string firstName , string lastName , string nationalCode)
        => new(firstName , lastName , nationalCode);
}
