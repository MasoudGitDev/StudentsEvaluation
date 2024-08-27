namespace Domains.School.Student.Aggregate;
public partial class Student {
    public ulong Id { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string NationalCode { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}

