using Konteh.Domain;
using Konteh.Infrastructure.Repositories;
using MediatR;

namespace Konteh.BackOfficeApi.Features.Questions;

public static class GetAllQuestions
{
    public class Query : IRequest<IEnumerable<Response>>;


    public class Response
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionCategory Category { get; set; }
        public QuestionType Type { get; set; }

    }

    public class RequestHandler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly IRepository<Question> _questionRepository;
        public RequestHandler(IRepository<Question> repository)
        {
            _questionRepository = repository;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAll();
            return questions.Select(q => new Response
            {
                Id = q.Id,
                Text = q.Text,
                Category = q.Category,
                Type = q.Type
            });
        }
    }
}
