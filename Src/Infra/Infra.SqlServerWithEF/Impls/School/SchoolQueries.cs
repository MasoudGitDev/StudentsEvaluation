﻿using Domains.School.Course.Repo;
using Domains.School.Shared.Abstractions;
using Domains.School.Student.Repo;
using Domains.School.Teacher.Repo;

namespace Infra.SqlServerWithEF.Impls.School;
internal sealed class SchoolQueries(
    IStudentQueries _studentQueries ,
    ICourseQueries _courseQueries ,
    ITeacherQueries _teacherQueries
    ) : ISchoolQueries {
    public IStudentQueries Students => _studentQueries;

    public ICourseQueries Courses => _courseQueries;

    public ITeacherQueries Teachers => _teacherQueries;
}
