using Domains.School.Abstractions;
using Domains.School.Student.Repo;

namespace Infra.SqlServerWithEF.Impls.School;
internal sealed class SchoolQueries(
    IStudentQueries _studentQueries
    ) : ISchoolQueries {
    public IStudentQueries Students => _studentQueries;
}
