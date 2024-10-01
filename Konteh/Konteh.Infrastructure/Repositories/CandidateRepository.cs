namespace Konteh.Infrastructure.Repositories;
using Konteh.Domain;

public class CandidateRepository : BaseRepository<Candidate>
{
    public CandidateRepository(KontehDbContext context) : base(context)
    {

    }
}
