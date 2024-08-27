namespace Domains.School.Course.Aggregate;
public partial class Course {
    public static Course New(ulong code , string name)
        => new() {
             Code = code ,
             Name = name
        };
}
