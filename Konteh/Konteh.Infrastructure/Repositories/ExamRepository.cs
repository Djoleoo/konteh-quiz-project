namespace Konteh.Infrastructure.Repositories;
using Konteh.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class ExamRepository : BaseRepository<Exam>, IExamRepository
{
    public ExamRepository(KontehDbContext context) : base(context)
    {

    }

    public override async Task<List<Exam>> GetAll()
    {
        return await _dbSet
            .Include(x => x.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .ThenInclude(sa => sa.Answers)
            .Include(x => x.ExamQuestions)
                .ThenInclude(eq => eq.SelectedAnswers)
            .Include(x => x.ExamQuestions)

            .ToListAsync();
    }

    public async Task<List<Exam>> GetByQuestion(int questionId)
    {
        return await _dbSet
            .Where(exam => exam.ExamQuestions.Any(eq => eq.Question.Id == questionId))
            .Include(exam => exam.ExamQuestions)
                .ThenInclude(eq => eq.Question)
            .Include(exam => exam.ExamQuestions)
                .ThenInclude(eq => eq.SelectedAnswers)
            .ToListAsync();
    }

    public async Task<List<Exam>> GetByQuestions(List<int> questionIds)
    {
        List<Exam> foundExams = new List<Exam>();

        foreach (var id in questionIds)
        {
            var exams = await _dbSet
              .Where(exam => exam.ExamQuestions.Any(eq => eq.Question.Id == id))
              .Include(exam => exam.ExamQuestions)
                  .ThenInclude(eq => eq.Question)
              .Include(exam => exam.ExamQuestions)
                  .ThenInclude(eq => eq.SelectedAnswers)
              .ToListAsync();

            foundExams.AddRange(exams);
        }
        return foundExams;
    }

    public override async Task<Exam?> GetById(int id)
    {
        var exam = await _dbSet
        .Include(eq => eq.ExamQuestions)
           .ThenInclude(eq => eq.Question)
               .ThenInclude(q => q.Answers)
        .Include(x => x.ExamQuestions)
           .ThenInclude(eq => eq.SelectedAnswers)
        .SingleOrDefaultAsync(x => x.Id == id);

        return exam;
    }
    public override IQueryable<Exam> Search(Expression<Func<Exam, bool>> predicate)
    {
        return _dbSet
            .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                    .ThenInclude(q => q.Answers)
            .Include(e => e.Candidate)
            .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.SelectedAnswers)
            .Where(predicate);
    }
}
