namespace Domains.School.Course.Aggregate;
public partial class Course {
    public static Course New(string code , string name , ulong teacherId)
        => new() {
             Code = code ,
             Name = name , 
             TeacherId = teacherId
        };
}
