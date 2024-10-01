using Konteh.BackOfficeApi;
using Konteh.BackOfficeApi.Features.Questions;
using Konteh.Domain;
using Konteh.Infrastructure.Repositories;
using Respawn;
using System.Net.Http.Json;

namespace Konteh.Tests;

public class CreateOrUpdateIntegrationTest : BaseIntegrationTest<Program>
{
    private static Respawner _respawner;
    private static string _connectionString = "Server=.;Database=KontehDBTest;Trusted_Connection=True;TrustServerCertificate=True;";
    private HttpClient _client;
    private readonly IRepository<Question> _questionRepository;

    public CreateOrUpdateIntegrationTest()
    {
        _questionRepository = Resolve<IRepository<Question>>();
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
    public async Task CreateQuestion_ShouldReturnSuccess()
    {
        var command = new CreateOrUpdateQuestion.Command
        {
            Text = "Pitanje Test",
            Type = QuestionType.Checkbox,
            Category = QuestionCategory.CSharp,
            Answers = new List<CreateOrUpdateQuestion.AnswerRequest>
            {
                new CreateOrUpdateQuestion.AnswerRequest { Text = "Answer 1", IsCorrect = true },
                new CreateOrUpdateQuestion.AnswerRequest { Text = "Answer 5", IsCorrect = true },
                new CreateOrUpdateQuestion.AnswerRequest { Text = "Answer 2", IsCorrect = false }
            }
        };

        var response = await _client.PostAsJsonAsync("api/questions", command);
        var questions = _questionRepository.GetAll().Result;
        var createdQuestion = questions.FirstOrDefault(q => q.Text == command.Text);

        Assert.That(response.IsSuccessStatusCode, Is.True, "Expected response status to be success.");
        await Verify(createdQuestion).IgnoreMember("Id");
    }
}
