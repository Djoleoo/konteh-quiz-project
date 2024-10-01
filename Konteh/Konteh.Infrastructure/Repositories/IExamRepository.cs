using Konteh.Domain;

namespace Konteh.Infrastructure.Repositories;

public interface IExamRepository
{
    Task<List<Exam>> GetByQuestion(int questionId);
    Task<List<Exam>> GetByQuestions(List<int> questionIds);
}
