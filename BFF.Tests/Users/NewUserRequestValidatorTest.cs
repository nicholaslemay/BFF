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

    [Fact]
    public void ValidatesGendersIsInValidChoices()
    {
        ValidateValuesAreValidForType(r=> r.gender, NewUserRequest.ValidGenders);
        ValidateValueIsInvalidForField(r=> r.gender, "SomeRandopmGender", "Invalid gender");
    }
    
    protected override NewUserRequest ValidExample() => 
        new("tony", "tony@gmail.com", "male", "active");
}