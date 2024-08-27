using Domains.School.Shared;

namespace Domains.School.Teacher.Aggregate;
public partial class Teacher : Person {
    public string PersonnelCode { get; private set; } = null!;
}
