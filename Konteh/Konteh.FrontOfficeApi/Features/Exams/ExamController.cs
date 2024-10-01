namespace Konteh.FrontOfficeApi.Features.Exams;

using MediatR;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/exams")]
public class ExamController : Controller
{
    private readonly IMediator _mediator;

    public ExamController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<GenerateExam.Response>> CreateExam([FromBody] GenerateExam.Command candidate)
    {
        var response = await _mediator.Send(candidate);
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<GetExam.Response>> GetExam(int id)
    {
        var response = await _mediator.Send(new GetExam.Query { Id = id });
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult> Submit(SubmitExam.Command request)
    {
        await _mediator.Send(request);
        return Ok();
    }
}

