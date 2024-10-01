using Konteh.BackOfficeApi.Features.Exams;
using Konteh.BackOfficeApi.HubConfig;
using Konteh.Domain;
using Konteh.Infrastructure.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Konteh.BackOfficeApi.Consumers;

public class ExamRequestedConsumer : IConsumer<ExamRequestedEvent>
{
    private readonly IHubContext<ExamHub> _hubContext;
    public ExamRequestedConsumer(IHubContext<ExamHub> hubContext)
    {
        _hubContext = hubContext;
    }
    public async Task Consume(ConsumeContext<ExamRequestedEvent> context)
    {
        var message = context.Message;
        await _hubContext.Clients.All.SendAsync("ReceiveExamRequest", new GetExams.Response
        {
            CandidateName = $"{message.Name} {message.Surname}",
            ExamStatus = ExamStatus.InProgress.ToString(),
            Id = message.Id,
            Score = $"0/{message.NumberOfQuestions}",
            StartTime = message.StartTime
        });
    }
}