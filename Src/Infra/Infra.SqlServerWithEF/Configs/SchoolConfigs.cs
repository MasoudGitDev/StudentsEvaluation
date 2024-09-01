using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.SqlServerWithEF.Configs;
internal class SchoolConfigs :
    IEntityTypeConfiguration<Student>,
    IEntityTypeConfiguration<Teacher>,
    IEntityTypeConfiguration<Course>,
    IEntityTypeConfiguration<ExamResult> {

    public void Configure(EntityTypeBuilder<Student> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.NationalCode).HasMaxLength(50);
        builder.HasIndex(x => x.NationalCode).IsUnique();

        builder.HasMany(x => x.Exams).WithOne(x => x.Student).IsRequired()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("Students");
    }

    public void Configure(EntityTypeBuilder<Teacher> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.PersonnelCode).HasMaxLength(50);
        builder.HasIndex(x => x.PersonnelCode).IsUnique();
        builder.ToTable("Teachers");

        builder.HasMany(x => x.Courses).WithOne(x => x.Teacher).IsRequired()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public void Configure(EntityTypeBuilder<Course> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Code).HasMaxLength(100);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.ToTable("Courses");

        builder.HasOne(x => x.Teacher).WithMany(x => x.Courses).IsRequired()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Exams).WithOne(x => x.Course).IsRequired()
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public void Configure(EntityTypeBuilder<ExamResult> builder) {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasIndex(x => new { x.StudentId , x.CourseId , x.TeacherId , x.ExamDateTime }).IsUnique();
        builder.ToTable("ExamResults");
    }
}
