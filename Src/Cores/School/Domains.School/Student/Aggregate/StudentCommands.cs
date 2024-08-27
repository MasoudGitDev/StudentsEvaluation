namespace Domains.School.Student.Aggregate;
public partial class Student {
    public static Student New(string firstName , string lastName , string nationalCode)
        => new() {
            FirstName = firstName ,
            LastName = lastName ,
            NationalCode = nationalCode
        };
}
