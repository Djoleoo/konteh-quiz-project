using Konteh.Domain;

namespace Konteh.Tests;

public class IsCorrectTests
{

    [Test]
    public void IsCorrect_ShouldReturnTrue_WhenAllSelectedAnswersAreCorrect()
    {
        var correctAnswer1 = new Answer { Id = 1, Text = "Correct Answer 1", IsCorrect = true };
        var correctAnswer2 = new Answer { Id = 2, Text = "Correct Answer 2", IsCorrect = true };
        var wrongAnswer = new Answer { Id = 3, Text = "Wrong Answer", IsCorrect = false };

        var question = new Question
        {
            Id = 1,
            Text = "Sample Question?",
            Answers = new List<Answer> { correctAnswer1, correctAnswer2, wrongAnswer }
        };

        var examQuestion = new ExamQuestion
        {
            Question = question,
            SelectedAnswers = new List<Answer> { correctAnswer1, correctAnswer2 }
        };

        var result = examQuestion.IsCorrect();

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsCorrect_ShouldReturnFalse_WhenAnySelectedAnswerIsWrong()
    {
        var correctAnswer = new Answer { Id = 1, Text = "Correct Answer", IsCorrect = true };
        var wrongAnswer = new Answer { Id = 2, Text = "Wrong Answer", IsCorrect = false };

        var question = new Question
        {
            Id = 1,
            Text = "Sample Question?",
            Answers = new List<Answer> { correctAnswer, wrongAnswer }
        };

        var examQuestion = new ExamQuestion
        {
            Question = question,
            SelectedAnswers = new List<Answer> { correctAnswer, wrongAnswer }
        };

        var result = examQuestion.IsCorrect();

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsCorrect_ShouldReturnFalse_WhenNoAnswersAreSelected()
    {
        var correctAnswer = new Answer { Id = 1, Text = "Correct Answer", IsCorrect = true };

        var question = new Question
        {
            Id = 1,
            Text = "Sample Question?",
            Answers = new List<Answer> { correctAnswer }
        };

        var examQuestion = new ExamQuestion
        {
            Question = question,
            SelectedAnswers = new List<Answer>()
        };

        var result = examQuestion.IsCorrect();

        Assert.That(result, Is.False);
    }


}
