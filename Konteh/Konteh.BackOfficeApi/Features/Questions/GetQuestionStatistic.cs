using Konteh.Domain;
using Konteh.Infrastructure.Exceptions;
using Konteh.Infrastructure.Repositories;
using MediatR;

namespace Konteh.BackOfficeApi.Features.Questions;

public class GetQuestionStatistic
{
    public class Query : IRequest<Response>
    {
        public int QuestionId { get; set; }
    }

    public class Response
    {
        public double Percentage { get; set; }
        public string Text { get; set; } = String.Empty;
    }
    public class RequestHandler : IRequestHandler<Query, Response>
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<ExamQuestion> _examQuestionRepository;

        public RequestHandler(IRepository<Question> questionRepository, IRepository<ExamQuestion> examQuestionRepository)
        {
            _questionRepository = questionRepository;
            _examQuestionRepository = examQuestionRepository;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var question = _questionRepository.GetById(request.QuestionId).Result;
            if (question == null)
            {
                throw new NotFoundException();
            }

            var examQuestions = await _examQuestionRepository.GetAll();

            int totalAttempts = 0;
            int correctAttempts = 0;

            var examQuestionForQuestion = examQuestions.Where(eq => eq.Question.Id == question.Id);

            totalAttempts = examQuestionForQuestion.Count();
            correctAttempts = examQuestionForQuestion.Count(x => x.IsCorrect());

            if (totalAttempts <= 0)
            {
                return new Response { Percentage = 0, Text = question!.Text };
            }
            else
            {
                var res = new Response { Percentage = Math.Round((double)correctAttempts / totalAttempts * 100, 2), Text = question.Text };
                return res;
            }
        }

    }
}
