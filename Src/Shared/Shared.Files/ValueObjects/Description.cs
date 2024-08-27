namespace Shared.Files.ValueObjects;
public record class Description {
    public string Value { get; private set; }
    public Description(string message) {
        Value = message;
    }
    public static Description New(string message) => new(message);
    public static implicit operator string(Description description) => description.Value;
    public static implicit operator Description(string value) => new(value);
}
