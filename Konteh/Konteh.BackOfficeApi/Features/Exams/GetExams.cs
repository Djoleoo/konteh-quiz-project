using Konteh.Domain;
using Konteh.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Konteh.BackOfficeApi.Features.Exams;

public static class GetExams
{
    public class Query : IRequest<IEnumerable<Response>>
    {
        public string? Text { get; set; } = string.Empty;
    }
    public class Response
    {
        public int Id { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string ExamStatus { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
    }
    public class RequestHandler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly IRepository<Exam> _examRepository;
        public RequestHandler(IRepository<Exam> examRepository)
        {
            _examRepository = examRepository;
        }

        public async Task<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            string searchText = "";
            if (request.Text != null)
            {
                searchText = request.Text.Trim();
            }

            var query = _examRepository.Search(exam =>
                exam.Candidate.Name.Contains(searchText) ||
                exam.Candidate.Surname.Contains(searchText));

            var length = query.Count();

            var exams = await query
                .OrderByDescending(exam => exam.StartTime)
                .ToListAsync();

            var examResult = exams.Select(exam => new Response
            {
                Id = exam.Id,
                CandidateName = $"{exam.Candidate.Name} {exam.Candidate.Surname}",
                Score = $"{exam.ExamQuestions.Count(eq => eq.IsCorrect())}/{exam.ExamQuestions.Count}",
                ExamStatus = exam.Status.ToString(),
                StartTime = exam.StartTime.ToLocalTime()
            });

            return examResult;
        }

    }
}




