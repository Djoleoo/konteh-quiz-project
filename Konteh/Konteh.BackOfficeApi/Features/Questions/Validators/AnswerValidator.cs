using FluentValidation;

namespace Konteh.BackOfficeApi.Features.Questions.Validators;

public class AnswerValidator : AbstractValidator<CreateOrUpdateQuestion.AnswerRequest>
{
    public AnswerValidator()
    {
        RuleFor(a => a.Text).NotEmpty().WithMessage("Answer text is required.");
    }
}
