namespace Domains.School.Course.Aggregate;
public partial class Course {
    public ulong Id { get; private set; }
    public ulong Code { get; private set; }
    public string Name { get; private set; } = null!;
}
