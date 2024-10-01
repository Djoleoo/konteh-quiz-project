namespace Konteh.Infrastructure.Repositories;

using Konteh.Domain;
using Microsoft.EntityFrameworkCore;

public class QuestionRepository : BaseRepository<Question>
{
    public QuestionRepository(KontehDbContext context) : base(context)
    {
    }
    public override void Delete(Question entity) => entity.IsDeleted = true;
    public override async Task<List<Question>> GetAll() => await _dbSet.Include(x => x.Answers.Where(a => !a.IsDeleted)).Where(a => !a.IsDeleted).ToListAsync();
    public override async Task<Question?> GetById(int id) => await _dbSet.Include(x => x.Answers.Where(a => !a.IsDeleted)).SingleOrDefaultAsync(x => x.Id == id);
}
