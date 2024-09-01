using Domains.School.Shared;
using ExamResultModel = Domains.School.ExamResult.Aggregate.ExamResult;

namespace Domains.School.Student.Aggregate;
public partial class Student : Person {
    public string NationalCode { get; private set; } = null!;


    //================relationships
    public ICollection<ExamResultModel> Exams { get; set; }
}

