namespace Shared.Files.ValueObjects;
public record Description(string Content) {

    public static implicit operator string(Description description) => description.Content;
    public static implicit operator Description(string content) => new(content);
}
