using Domains.School.Student.Repo;

namespace Domains.School.Abstractions;
public interface ISchoolQueries {
    public IStudentQueries Students { get; }
}
