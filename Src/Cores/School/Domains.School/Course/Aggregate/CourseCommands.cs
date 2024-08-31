using TeacherModel = Domains.School.Teacher.Aggregate.Teacher;

namespace Domains.School.Course.Aggregate;
public partial class Course {
    public static Course New(string code , string name , ulong teacherId)
        => new() {
            Code = code ,
            Name = name ,
            TeacherId = teacherId
        };

    public static Course New(ulong id , string code , string name , ulong teacherId)
        => new() {
            Id = id ,
            Code = code ,
            Name = name ,
            TeacherId = teacherId
        };

    public static Course New(ulong id , string code , string name , TeacherModel teacher)
      => new() {
          Id = id ,
          Code = code ,
          Name = name ,
          TeacherId = teacher.Id,
          Teacher = teacher          
      };
}
