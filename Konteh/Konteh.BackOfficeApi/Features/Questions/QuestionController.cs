using Konteh.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Konteh.BackOfficeApi.Features.Questions;

[Authorize]
[ApiController]
[Route("api/questions")]
public class QuestionController : Controller
{
    private readonly IMediator _mediator;

    public QuestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAllQuestions.Response>>> GetAll()
    {
        var response = await _mediator.Send(new GetAllQuestions.Query());
        return Ok(response);
    }


    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteQuestion.Command { Id = id });
        return Ok();
    }

    [HttpPut]
    [Route("search")]
    public async Task<ActionResult<SearchQuestions.SearchResponse>> Search(SearchQuestions.Query request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrUpdate(CreateOrUpdateQuestion.Command command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
    public async Task<ActionResult<Question>> GetQuestionById(int id)
    {
        var response = await _mediator.Send(new GetQuestionById.Query { Id = id });
        return Ok(response);
    }

    [HttpGet("statistic/{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(GetQuestionStatistic.Response), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetQuestionStatistic.Response>> GetQuestionStatistic(int id)
    {
        var response = await _mediator.Send(new GetQuestionStatistic.Query { QuestionId = id });
        return Ok(response);
    }

    [HttpGet("category/statistics")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(IEnumerable<GetQuestionCategoryStatistic.Response>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GetQuestionCategoryStatistic.Response>>> GetCategoryQuestionStatistic()
    {
        var response = await _mediator.Send(new GetQuestionCategoryStatistic.Query { });
        return Ok(response);
    }

}
