using Konteh.Domain;
using Konteh.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Konteh.Infrastructure.BackgroundJobs;

public class ExpiredExamsCleanerJob : IJob
{
    private readonly IRepository<Exam> _repository;

    public ExpiredExamsCleanerJob(IRepository<Exam> repository)
    {
        _repository = repository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var exams = await _repository.Search(a => a.Status == ExamStatus.InProgress && a.StartTime.AddHours(1) < DateTime.UtcNow).ToListAsync();
        if (!exams.Any())
            return;
        foreach (var exam in exams)
        {
            exam.Status = ExamStatus.Invalid;
        }
        await _repository.SaveChanges();
    }
}
