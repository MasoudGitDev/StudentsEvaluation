using Shared.Files.Constants;

namespace Shared.Files.Models;
public record MessageDescription(string Code , string Message , AlertType AlertType) {
    public static MessageDescription New(string code , string message , AlertType alertType)
        => new(code , message , alertType);

    public static MessageDescription Error(string code , string message)
       => new(code , message , AlertType.Error);

    public static MessageDescription Success(string code , string message)
       => new(code , message , AlertType.Success);

    public static MessageDescription Warning(string code , string message)
      => new(code , message , AlertType.Warning);
}
