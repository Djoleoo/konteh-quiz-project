namespace Konteh.Domain;

public class Answer
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public bool IsDeleted { get; set; } = false;

}
