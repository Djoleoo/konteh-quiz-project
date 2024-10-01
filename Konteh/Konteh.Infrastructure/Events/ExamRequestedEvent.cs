namespace Konteh.Infrastructure.Events;
public class ExamRequestedEvent
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int NumberOfQuestions { get; set; }
    public DateTime StartTime { get; set; }
    public int Id { get; set; }
}
