using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.SqlServerWithEF.Configs;
internal class SchoolConfigs :
    IEntityTypeConfiguration<Student> ,
    IEntityTypeConfiguration<Teacher> ,
    IEntityTypeConfiguration<Course> ,
    IEntityTypeConfiguration<ExamResult> {
    public void Configure(EntityTypeBuilder<Student> builder) {
        builder.Property(x=> x.Id).ValueGeneratedOnAdd();
        builder.ToTable("Students");
    }

    public void Configure(EntityTypeBuilder<Teacher> builder) {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.ToTable("Teachers");
    }

    public void Configure(EntityTypeBuilder<ExamResult> builder) {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.ToTable("ExamResults");
    }

    public void Configure(EntityTypeBuilder<Course> builder) {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.ToTable("Courses");
    }
}
