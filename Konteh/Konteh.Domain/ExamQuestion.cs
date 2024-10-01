namespace Konteh.Domain;

public class ExamQuestion
{
    public int Id { get; set; }
    public Question Question { get; set; } = null!;
    public IEnumerable<Answer> SelectedAnswers { get; set; } = new List<Answer>();

    public bool IsCorrect()
    {
        if (SelectedAnswers.Count() == 0)
        {
            return false;
        }
        var rightAnswers = Question.Answers.Where(answer => answer.IsCorrect).ToList();

        return AreSelectedAnswersCorrect(SelectedAnswers, rightAnswers);
    }

    private bool AreSelectedAnswersCorrect(IEnumerable<Answer> selectedAnswers, List<Answer> rightAnswers)
    {
        return selectedAnswers.Count() == rightAnswers.Count && !selectedAnswers.Except(rightAnswers).Any() && !rightAnswers.Except(selectedAnswers).Any();
    }
}
