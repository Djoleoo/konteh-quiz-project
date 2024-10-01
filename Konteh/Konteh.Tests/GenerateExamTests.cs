using Konteh.Domain;
using Konteh.FrontOfficeApi.Features.Exams;
using Konteh.Infrastructure.Repositories;
using MassTransit;
using NSubstitute;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Konteh.Tests;

public class GenerateExamTests
{
    private IRepository<Question> _questionRepositoryMock;
    private IRepository<ExamQuestion> _examQuestionRepositoryMock;
    private IRepository<Exam> _examRepositoryMock;
    private IRepository<Candidate> _candidateRepositoryMock;
    private IRandomNumberGenerator _rand;
    private GenerateExam.RequestHandler _handler;
    private IPublishEndpoint _publishEndpoint;

    [SetUp]
    public void Setup()
    {
        _questionRepositoryMock = Substitute.For<IRepository<Question>>();
        _examQuestionRepositoryMock = Substitute.For<IRepository<ExamQuestion>>();
        _examRepositoryMock = Substitute.For<IRepository<Exam>>();
        _candidateRepositoryMock = Substitute.For<IRepository<Candidate>>();
        _rand = Substitute.For<IRandomNumberGenerator>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _handler = new GenerateExam.RequestHandler(
            _questionRepositoryMock,
            _examRepositoryMock,
            _candidateRepositoryMock,
            _rand,
            _publishEndpoint);
    }

    [Test]
    public void GenerateExam_ShouldThrowException_WhenCandidateHasAlreadyTakenTest()
    {
        var query = new GenerateExam.Command
        {
            Email = "loncardjole@gmail.com",
            Name = "Djordje",
            Surname = "Loncar"
        };

        var existingExams = new List<Exam>
        {
            new Exam { Candidate = new Candidate { Email = "loncardjole@gmail.com" } }
        };

        MockExamRepositorySearch(existingExams);

        var exception = Assert.ThrowsAsync<ValidationException>(async () =>
            await _handler!.Handle(query, CancellationToken.None));

        Assert.That(exception.Message, Is.EqualTo("Candidate has already taken the test."));
    }

    [Test]
    public async Task GenerateExam_ShouldCreateNewCandidate_WhenCandidateDoesNotExist()
    {
        var request = new GenerateExam.Command
        {
            Email = "test@test.com",
            Name = "Kristina",
            Surname = "Zelic"
        };

        MockExamRepositorySearch(new List<Exam>());

        _questionRepositoryMock
            .GetAll()
            .Returns(PrepareQuestions());

        var response = await _handler!.Handle(request, CancellationToken.None);

        _candidateRepositoryMock
            .Received(1)
            .Add(Arg.Is<Candidate>(c => c.Email == request.Email && c.Name == request.Name && c.Surname == request.Surname));
    }

    [Test]
    public async Task GenerateExam_ShouldReturnCorrectResponse()
    {
        var request = new GenerateExam.Command { Email = "testnovi@test.com" };
        var questions = PrepareQuestions();

        MockExamRepositorySearch(new List<Exam>());

        _questionRepositoryMock
            .GetAll()
            .Returns(questions);

        _rand.Next(Arg.Any<int>()).Returns(0);
        var response = await _handler!.Handle(request, CancellationToken.None);

        await Verify(response)
            .IgnoreMember("Id");
    }

    private void MockExamRepositorySearch(List<Exam> exams)
    {
        _examRepositoryMock
            .Search(Arg.Any<Expression<Func<Exam, bool>>>())
            .Returns(exams.AsQueryable());
    }

    private List<Question> PrepareQuestions()
    {
        return new List<Question>
    {
        new Question { Id = 1,Text = "Question 1", Category = QuestionCategory.Http },
        new Question { Id = 2,Text = "Question 2", Category = QuestionCategory.Http },
        new Question { Id = 3,Text = "Question 3", Category = QuestionCategory.CSharp },
        new Question { Id = 4,Text = "Question 4", Category = QuestionCategory.CSharp },
        new Question { Id = 5,Text = "Question 5", Category = QuestionCategory.Sql },
        new Question { Id = 6,Text = "Question 6", Category = QuestionCategory.Sql },
        new Question { Id = 7,Text = "Question 7", Category = QuestionCategory.Oop },
        new Question { Id = 8,Text = "Question 8", Category = QuestionCategory.Oop },
        new Question { Id = 9,Text = "Question 9", Category = QuestionCategory.Git },
        new Question { Id = 10, Text = "Question 10", Category = QuestionCategory.Git },
        new Question { Id = 11, Text = "Question 11", Category = QuestionCategory.Http },
        new Question { Id = 12, Text = "Question 12", Category = QuestionCategory.CSharp },
        new Question { Id = 13, Text = "Question 13", Category = QuestionCategory.Sql },
        new Question { Id = 14, Text = "Question 14", Category = QuestionCategory.Oop },
        new Question { Id = 15, Text = "Question 15", Category = QuestionCategory.Git }
    };
    }

}