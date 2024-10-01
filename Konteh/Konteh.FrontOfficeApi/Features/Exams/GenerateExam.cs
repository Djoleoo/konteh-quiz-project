namespace Konteh.FrontOfficeApi.Features.Exams;

using Konteh.Domain;
using Konteh.Infrastructure.Events;
using Konteh.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using System.ComponentModel.DataAnnotations;

public static class GenerateExam
{
    public class Command : IRequest<Response>
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class Response
    {
        public int Id { get; set; }
        public List<ExamQuestionDto> ExamQuestions { get; set; } = [];
    }

    public class ExamQuestionDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public List<AnswerDto> SelectedAnswers { get; set; } = new();
    }

    public class AnswerDto
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; } = string.Empty;
    }


    public class RequestHandler : IRequestHandler<Command, Response>
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Exam> _examRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRandomNumberGenerator _random;
        private readonly IPublishEndpoint _publishEndpoint;

        public RequestHandler(IRepository<Question> questionRepository,
                              IRepository<Exam> examRepository, IRepository<Candidate> candidateRepository,
                              IRandomNumberGenerator random, IPublishEndpoint publishEndpoint)
        {
            _questionRepository = questionRepository;
            _examRepository = examRepository;
            _candidateRepository = candidateRepository;
            _random = random;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var candidate = CheckIfCandidateHasTakenTest(request);

            int numberOfQuestionsPerCategory = 2;
            var questions = await GetQuestionsForExam(numberOfQuestionsPerCategory);

            var exam = new Exam
            {
                Candidate = candidate,
                ExamQuestions = new List<ExamQuestion>(),
                Status = ExamStatus.InProgress,
                StartTime = DateTime.UtcNow
            };

            exam.ExamQuestions = questions
                .Select(question => new ExamQuestion
                {
                    Question = question,
                    SelectedAnswers = new List<Answer>()
                })
                .ToList();

            _examRepository.Add(exam);
            await _examRepository.SaveChanges();

            await _publishEndpoint.Publish(new ExamRequestedEvent
            {
                Name = candidate.Name,
                Surname = candidate.Surname,
                NumberOfQuestions = exam.ExamQuestions.Count,
                StartTime = exam.StartTime,
                Id = exam.Id
            });

            var examQuestionDtos = exam.ExamQuestions.Select(eq => new ExamQuestionDto
            {
                QuestionId = eq.Question.Id,
                QuestionText = eq.Question.Text,
                SelectedAnswers = new List<AnswerDto>()
            }).ToList();

            return new Response
            {
                Id = exam.Id,
                ExamQuestions = examQuestionDtos
            };
        }

        private Candidate CheckIfCandidateHasTakenTest(Command request)
        {
            var existingExam = _examRepository.Search(e => e.Candidate.Email == request.Email).ToList();

            if (existingExam.Any())
            {
                throw new ValidationException("Candidate has already taken the test.");
            }

            var candidate = new Candidate
            {
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname
            };

            _candidateRepository.Add(candidate);

            return candidate;
        }

        private async Task<List<Question>> GetQuestionsForExam(int numberOfQuestionsPerCategory)
        {
            var categories = Enum.GetValues(typeof(QuestionCategory)).Cast<QuestionCategory>();
            var allQuestions = await _questionRepository.GetAll();
            var questions = new List<Question>();

            foreach (var category in categories)
            {
                var randomQuestionsPerCategory = new List<Question>();
                var allQuestionsFromCategory = allQuestions
                    .Where(x => x.Category == category)
                    .ToList();

                if (numberOfQuestionsPerCategory > allQuestionsFromCategory.Count())
                {
                    throw new InvalidOperationException($"Not enough questions in {category} category.");
                }

                for (int i = 0; i < numberOfQuestionsPerCategory; i++)
                {
                    var randomQuestion = allQuestionsFromCategory[_random.Next(allQuestionsFromCategory.Count)];
                    allQuestionsFromCategory.Remove(randomQuestion);
                    randomQuestionsPerCategory.Add(randomQuestion);
                }

                questions.AddRange(randomQuestionsPerCategory);
            }

            return questions;
        }

    }

}

