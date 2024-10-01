using Konteh.Domain;
using Konteh.FrontOfficeApi;
using Konteh.FrontOfficeApi.Features.Exams;
using Konteh.Infrastructure;
using Respawn;
using System.Net;
using System.Net.Http.Json;

namespace Konteh.Tests;


public class GenerateExamIntegrationTests : BaseIntegrationTest<Program>
{
    private static Respawner _respawner;
    private static string _connectionString = "Server=.;Database=KontehDBTest;Trusted_Connection=True;TrustServerCertificate=True;";
    private HttpClient _client;

    public GenerateExamIntegrationTests()
    {
    }

    [SetUp]
    public async Task Setup()
    {
        _client = GetClient();
        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
        await _respawner.ResetAsync(_connectionString);
    }

    [TearDown]
    public void RunAfterTest()
    {
        _client.Dispose();
    }

    [Test]
    public async Task CreateExam_Returns_ExamResponse()
    {
        var context = Resolve<KontehDbContext>();
        context.Questions.AddRange(PrepareQuestions());
        await context.SaveChangesAsync();

        var candidateRequest = new GenerateExam.Command
        {
            Name = "Djordje",
            Surname = "Loncar",
            Email = "loncardjole@gmail.com"
        };
        var _client = GetClient();
        var response = await _client.PostAsJsonAsync("api/exams", candidateRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var examResponse = await response.Content.ReadFromJsonAsync<GenerateExam.Response>();
        Assert.That(examResponse, Is.Not.Null);
        Assert.That(examResponse.ExamQuestions.Count, Is.EqualTo(10));
    }

    private List<Question> PrepareQuestions()
    {
        return new List<Question>
        {
            new Question { Text = "Question 1", Category = QuestionCategory.Http },
            new Question { Text = "Question 2", Category = QuestionCategory.Http },
            new Question { Text = "Question 3", Category = QuestionCategory.CSharp },
            new Question { Text = "Question 4", Category = QuestionCategory.CSharp },
            new Question { Text = "Question 5", Category = QuestionCategory.Sql },
            new Question { Text = "Question 6", Category = QuestionCategory.Sql },
            new Question { Text = "Question 7", Category = QuestionCategory.Oop },
            new Question { Text = "Question 8", Category = QuestionCategory.Oop },
            new Question { Text = "Question 9", Category = QuestionCategory.Git },
            new Question { Text = "Question 10", Category = QuestionCategory.Git },
            new Question { Text = "Question 11", Category = QuestionCategory.Http },
            new Question { Text = "Question 12", Category = QuestionCategory.CSharp },
            new Question { Text = "Question 13", Category = QuestionCategory.Sql },
            new Question { Text = "Question 14", Category = QuestionCategory.Oop },
            new Question { Text = "Question 15", Category = QuestionCategory.Git }
        };
    }
}
