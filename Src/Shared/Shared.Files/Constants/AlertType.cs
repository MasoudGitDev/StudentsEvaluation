namespace Shared.Files.Constants;
public record AlertType(string Name) {
    public static AlertType Error => new(nameof(Error));
    public static AlertType Warning => new(nameof(Warning));
    public static AlertType Success => new(nameof(Success));
}
