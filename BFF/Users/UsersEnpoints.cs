using BFF.Support;
using FluentValidation;

namespace BFF.Users;

public record NewUserRequest(string? name, string? email, string? gender, string? status);
public record NewUserResponse(int id);
public static class UsersEnpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/public/v2/users", (IValidator<NewUserRequest> validator, NewUserRequest request) =>
            {
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                    return Results.ValidationProblem(validationResult.ToDictionary());

                return Results.Ok(new NewUserResponse( new Random().Next()));
            })
            .Produces(200)
            .Produces(400);

        return app;
    }
}