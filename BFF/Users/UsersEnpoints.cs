using BFF.Communications;
using BFF.Support;
using FluentValidation;
using static BFF.Users.GenderType;
using static BFF.Users.NewUserRequest;
using static Microsoft.AspNetCore.Http.Results;

namespace BFF.Users;
public static class UsersEnpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/public/v2/users", async (IValidator<NewUserRequest> validator, NewUserRequest request, IUserRepository userRepository, ICommunicationServiceClient communicationServiceClient) =>
            {
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                    return ValidationProblem(validationResult.ToDictionary());
                
                var user = request.AsUser();
                var userId = await userRepository.AddUser(user);

                await communicationServiceClient.SendAccountCreationConfirmationAsync(new AccountCreationConfirmation(user.Email, user.Name));
                
                return Ok(new NewUserResponse(userId));
            })
            .Produces(200)
            .Produces(400);

        return app;
    }
}

public class NewUserRequestValidator : AbstractValidator<NewUserRequest>
{
    public NewUserRequestValidator()
    {
        RuleFor(x => x.email).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.name).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.gender).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.status).NotEmpty().WithMessage(Messages.RequiredValue);
        RuleFor(x => x.gender).Must(g => ValidGenders.Contains(g)).WithMessage("Invalid gender");
    }
}

public record NewUserRequest(string? name, string? email, string? gender, string? status)
{
    private static readonly Dictionary<string, GenderType> GendersMapping = new(){{"female", Female}, {"male", Male}, {"other", Other}};
    public static readonly IReadOnlyList<string> ValidGenders = GendersMapping.Keys.ToList();
    
    public User AsUser() =>
        new()
        {
            Name = name,
            Email = email,
            Gender = GendersMapping[gender]
        };
}

public record NewUserResponse(int id);


