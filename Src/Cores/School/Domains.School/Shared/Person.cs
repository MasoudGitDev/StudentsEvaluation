namespace Domains.School.Shared;
public class Person {
    public ulong Id { get; protected set; }
    public string FirstName { get; protected set; } = null!;
    public string LastName { get; protected set; } = null!;
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; protected set; }

}
