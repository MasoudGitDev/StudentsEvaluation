using Domains.School.Abstractions;
using TeacherModel= Domains.School.Teacher.Aggregate.Teacher;
using Shared.Files.Exceptions;
using CourseModel =Domains.School.Course.Aggregate.Course;

namespace Domains.School.Shared.Extensions;
public static class AggregateExtensions {
    public static ISchoolUOW MustHasValue(this ISchoolUOW? unitOfWork) {
        if(unitOfWork is null){
            throw new CustomException("Null-Object" , "The School unit of work must has value.");
        }
        return unitOfWork;
    }

    public static List<TeacherModel> CreateFakeList(this List<TeacherModel> teachers , int count = 60) {
        for(int i = 1 ; i <= count ; i++) {
            teachers.Add(TeacherModel.New("Teacher_FN_1" , "Teacher_LN_1" , "Teacher_PC_1"));
        }
        return teachers;
    }

    public static List<CourseModel> CreateFakeList(this List<CourseModel> courses , int count = 60) {
        for(int i = 1 ; i <= count ; i++) {
            courses.Add(CourseModel.New("Course_Code_1" , "Course_Name_1" , 0));
        }
        return courses;
    }
}
