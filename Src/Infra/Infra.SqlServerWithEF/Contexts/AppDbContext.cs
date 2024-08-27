using Domains.School.Course.Aggregate;
using Domains.School.ExamResult.Aggregate;
using Domains.School.Student.Aggregate;
using Domains.School.Teacher.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Contexts;
internal class AppDbContext : DbContext {

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {
        
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }
}
