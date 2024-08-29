namespace Shared.Files.Constants;
public static class MessageResults {
    // teacher
    public const string FoundTeacher = "A teacher with personnel code : <{0}> found.";
    public const string CreateTeacher = "The new teacher with personnel code : <{0}> has been created successfully.";
    public const string NotFoundTeacher = "The teacher with personnel code : <{0}> not found.";

    // student
    public const string CreateStudent = "The new student with national code : <{0}> has been created successfully.";
    public const string FoundStudent = "The student with national code : <{0}> found.";
    public const string NotFoundStudent = "The student with national code : <{0}> not found.";

    // exam
    public const string DateTimeError = "The date and time must be less than now.";
    public const string InvalidScore = "The score must be within the range of 0 to 20. Provided score: {0}.";
    public const string OneExamPerCoursePerDay = "Each student can only take one exam per course per day.";
    public const string CreateExamResult = "The exam result was created successfully.";

    // course
    public const string CreateCourse = "The new course with code: <{0}> has been created successfully.";
    public const string FoundCourse = "A course with code: <{0}> already exists.";
    public const string NotFoundCourse = "The course with code : <{0}> not found.";
}