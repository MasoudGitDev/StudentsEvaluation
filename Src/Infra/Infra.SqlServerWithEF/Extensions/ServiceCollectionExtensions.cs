using Domains.School.Abstractions;
using Domains.School.Course.Repo;
using Domains.School.ExamResult.Repo;
using Domains.School.Shared.Abstractions;
using Domains.School.Student.Repo;
using Domains.School.Teacher.Repo;
using Infra.SqlServerWithEF.Contexts;
using Infra.SqlServerWithEF.DummyData;
using Infra.SqlServerWithEF.Impls.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Files.Extensions;

namespace Infra.SqlServerWithEF.Extensions;
public static class ServiceCollectionExtensions {
    public static async Task<IServiceCollection> AddInfraLayerServices(this IServiceCollection services) {

        var configuration= services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddDbContext<AppDbContext>(builder => {
            builder.UseSqlServer(configuration.GetDefaultConnectionString());
            builder.EnableSensitiveDataLogging();
        });

        // school services
        services.AddScoped<IStudentQueries , StudentQueries>();
        services.AddScoped<ITeacherQueries , TeacherQueries>();
        services.AddScoped<ICourseQueries , CourseQueries>();
        services.AddScoped<IExamResultQueries , ExamResultQueries>();

        services.AddScoped<ISchoolQueries , SchoolQueries>();
        services.AddScoped<ISchoolUOW , SchoolUnitOfWork>();

        // ====== add dummy data
        ISchoolUOW unitOfWork = services.BuildServiceProvider().GetRequiredService<ISchoolUOW>();
        var dummyData = new ProjectDummyData(unitOfWork);
        await dummyData.ExecuteAsync();
        return services;
    }
}
