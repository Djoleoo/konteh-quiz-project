namespace Konteh.Infrastructure.Repositories;
using Konteh.Domain;
using Microsoft.EntityFrameworkCore;

public class ExamQuestionRepository : BaseRepository<ExamQuestion>
{
    public ExamQuestionRepository(KontehDbContext context) : base(context)
    {
    }

    public override async Task<ExamQuestion?> GetById(int id) => await _dbSet.Include(eq => eq.Question).ThenInclude(q => q.Answers).SingleOrDefaultAsync(eq => eq.Id == id);

    public override IEnumerable<ExamQuestion> GetByIds(List<int> ids) => _dbSet.Include(eq => eq.Question).ThenInclude(q => q.Answers).Where(eq => ids.Contains(eq.Id));
    public override Task<List<ExamQuestion>> GetAll() => _dbSet.Include(eq => eq.Question).ThenInclude(q => q.Answers).Include(eq => eq.SelectedAnswers).ToListAsync();
}
