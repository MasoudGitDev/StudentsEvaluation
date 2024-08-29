namespace Domains.School.Student.Aggregate;
public partial class Student {
    public static Student New(string firstName , string lastName , string nationalCode)
        => new() {
            FirstName = firstName ,
            LastName = lastName ,
            NationalCode = nationalCode
        };

    public static Student New(ulong id , string firstName , string lastName , string nationalCode)
       => new() {
           Id = id ,
           FirstName = firstName ,
           LastName = lastName ,
           NationalCode = nationalCode,           
       };
}
