using Domains.School.Shared;

namespace Domains.School.Student.Aggregate;
public partial class Student : Person {
    public string NationalCode { get; private set; } = null!;
}

