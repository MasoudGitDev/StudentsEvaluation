﻿using Domains.School.ExamResult.Aggregate;
using Domains.School.ExamResult.Repo;
using Infra.SqlServerWithEF.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.SqlServerWithEF.Impls.School;
internal class ExamResultQueries(AppDbContext _dbContext) : IExamResultQueries {
    public async Task<List<ExamResult>> GetAllAsync() {
        return await _dbContext.ExamResults.ToListAsync();
    }

    public async Task<ExamResult?> GetByIdAsync(ulong id) {
        return await _dbContext.ExamResults.FirstOrDefaultAsync(x => x.Id == id);
    }
}
