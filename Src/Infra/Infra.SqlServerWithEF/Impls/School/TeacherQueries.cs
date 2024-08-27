using Domains.School.Teacher.Aggregate;
using Domains.School.Teacher.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Impls.School;
internal class TeacherQueries(AppDbContext _dbContext) : ITeacherQueries {
    public async Task<List<Teacher>> GetAllAsync() {
        return await _dbContext.Teachers.ToListAsync();
    }

    public async Task<Teacher?> GetByIdAsync(ulong id) {
        return await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Teacher?> GetByPersonnelCodeAsync(string personnelCode) {
        return await _dbContext.Teachers.FirstOrDefaultAsync(x => x.PersonnelCode == personnelCode);
    }
}
