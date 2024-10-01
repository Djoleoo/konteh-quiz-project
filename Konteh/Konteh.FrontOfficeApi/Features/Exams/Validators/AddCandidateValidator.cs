using FluentValidation;

namespace Konteh.FrontOfficeApi.Features.Exams.Validators
{
    public class AddCandidateValidator : AbstractValidator<GenerateExam.Command>
    {
        public AddCandidateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Email must be in a valid format.");
        }
    }
}
