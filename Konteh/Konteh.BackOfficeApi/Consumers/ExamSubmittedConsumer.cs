using Konteh.BackOfficeApi.Features.Exams;
using Konteh.BackOfficeApi.HubConfig;
using Konteh.Domain;
using Konteh.Infrastructure.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Konteh.BackOfficeApi.Consumers;

public class ExamSubmittedConsumer : IConsumer<ExamSubmittedEvent>
{
    private readonly IHubContext<ExamHub> _hubContext;
    public ExamSubmittedConsumer(IHubContext<ExamHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<ExamSubmittedEvent> context)
    {
        var message = context.Message;
        await _hubContext.Clients.All.SendAsync("RecieveExamSubmit", new GetExams.Response
        {
            Id = message.Id,
            Score = message.Score,
            ExamStatus = ExamStatus.Completed.ToString()
        });
    }
}
