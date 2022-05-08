using BFF.Support;
using FluentValidation;

namespace BFF.Users;

public class NewUserRequestValidator : AbstractValidator<NewUserRequest>
{
    public NewUserRequestValidator()
    {
        RuleFor(x => x.email).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.name).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.gender).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.status).NotEmpty().WithMessage(Messages.RequiredValue);
    }
}