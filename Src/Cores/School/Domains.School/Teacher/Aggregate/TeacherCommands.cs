namespace Domains.School.Teacher.Aggregate;
public partial class Teacher {
    public static Teacher New(string firstName , string lastName , string personnelCode)
        => new() {
            FirstName = firstName ,
            LastName = lastName ,
            PersonnelCode = personnelCode
        };
    public static Teacher New(ulong id , string firstName , string lastName , string personnelCode)
        => new() {
            Id = id ,
            FirstName = firstName ,
            LastName = lastName ,
            PersonnelCode = personnelCode
        };
}
