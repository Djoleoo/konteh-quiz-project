using FluentValidation;

namespace Konteh.BackOfficeApi.Features.Questions.Validators;

public class GetQuestionByIdValidator : AbstractValidator<GetQuestionById.Query>
{
    public GetQuestionByIdValidator()
    {
        RuleFor(query => query.Id)
                .GreaterThan(0).WithMessage("The question ID must be greater than 0.");
    }
}
