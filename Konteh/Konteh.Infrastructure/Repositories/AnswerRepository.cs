using Konteh.Domain;
using Microsoft.EntityFrameworkCore;

namespace Konteh.Infrastructure.Repositories;
public class AnswerRepository : BaseRepository<Answer>
{
    public AnswerRepository(KontehDbContext context) : base(context)
    {

    }
    public override async Task<Answer?> GetById(int id) => await _dbSet.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    public override void Delete(Answer entity) => entity.IsDeleted = true;
    public override async Task<List<Answer>> GetAll() => await _dbSet.Where(a => !a.IsDeleted).ToListAsync();
}
