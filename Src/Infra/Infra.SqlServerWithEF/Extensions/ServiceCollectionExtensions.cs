﻿using Domains.School.Abstractions;
using Domains.School.Course.Repo;
using Domains.School.Shared.Abstractions;
using Domains.School.Student.Repo;
using Domains.School.Teacher.Repo;
using Infra.SqlServerWithEF.Contexts;
using Infra.SqlServerWithEF.DummyData;
using Infra.SqlServerWithEF.Impls.School;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<ISchoolQueries , SchoolQueries>();
        services.AddScoped<ISchoolUOW , SchoolUnitOfWork>();

        // ====== add dummy data
        ISchoolUOW unitOfWork = services.BuildServiceProvider().GetRequiredService<ISchoolUOW>();
        AppDbContext dbContext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
        var dummyData = new ProjectDummyData(unitOfWork,dbContext);
        await dummyData.ExecuteAsync();
        return services;
    }
}
