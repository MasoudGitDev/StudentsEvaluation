namespace Shared.Files.Exceptions;
public class CustomException : Exception {
    public string Code { get; private set; } = "<unknown-code>";
    public string Description { get; private set; } = "<unknown-description";

    public CustomException(string code , string description) {
        Code = code;
        Description = description;
    }
}
