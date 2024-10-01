
namespace Konteh.Infrastructure.Events;

public class ExamSubmittedEvent
{
    public string Score { get; set; } = string.Empty;
    public int Id { get; set; }
}
