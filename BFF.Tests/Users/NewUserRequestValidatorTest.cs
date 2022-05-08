using System.ComponentModel.DataAnnotations;
using BFF.Users;
using Xunit;

namespace BFF.Tests.Users;

public class NewUserRequestValidatorTest : ValidationTest<NewUserRequestValidator, NewUserRequest>
{
    [Fact]
    public void HasNoErrorWhenValidExample() => ValidExempleHasNoError();
    
    [Fact]
    public void ValidatesPresenceOfEachRequiredFields()
    {
        ValidateFieldCannotBeNullOrEmpty(r=> r.name);
        ValidateFieldCannotBeNullOrEmpty(r=> r.email);
        ValidateFieldCannotBeNullOrEmpty(r=> r.gender);
        ValidateFieldCannotBeNullOrEmpty(r=> r.status);
    }

    protected override NewUserRequest ValidExample() => 
        new("tony", "tony@gmail.com", "male", "active");
}