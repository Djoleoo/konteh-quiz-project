using Konteh.Domain;
using Konteh.Infrastructure.Repositories;
using MediatR;

namespace Konteh.BackOfficeApi.Features.Questions;

public static class GetQuestionCategoryStatistic
{
    public class Query : IRequest<Response[]>;

    public class Response
    {
        public string CategoryName { get; set; } = string.Empty;
        public double CorrectPercentage { get; set; }
    }

    public class RequestHandler : IRequestHandler<Query, Response[]>
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<ExamQuestion> _examQuestionRepository;
        private readonly IExamRepository _examRepository;

        public RequestHandler(IRepository<Question> repository, IExamRepository examRepository, IRepository<ExamQuestion> examQuestionRepository)
        {
            _questionRepository = repository;
            _examRepository = examRepository;
            _examQuestionRepository = examQuestionRepository;
        }

        public async Task<Response[]> Handle(Query request, CancellationToken cancellationToken)
        {
            var categoryStatistics = new List<Response>();

            var examQuestions = await _examQuestionRepository.GetAll();
            var gropedExamQuestions = examQuestions.GroupBy(eq => eq.Question.Category);
            foreach (var group in gropedExamQuestions)
            {
                var totalAttempts = group.Count();
                var correctAttempts = 0;

                foreach (var examQuestion in group)
                {
                    if (examQuestion.IsCorrect())
                    {
                        correctAttempts++;
                    }
                }

                double correctPercentage = 0;

                if (totalAttempts > 0)
                {
                    correctPercentage = Math.Round((double)correctAttempts / totalAttempts * 100, 2);
                }

                categoryStatistics.Add(new Response
                {
                    CategoryName = group.Key.ToString(),
                    CorrectPercentage = correctPercentage
                });

            }

            return categoryStatistics.ToArray();
        }

    }
}
