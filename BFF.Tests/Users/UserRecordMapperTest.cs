using BFF.Users;
using FluentAssertions;
using Xunit;

namespace BFF.Tests.Users;

public class UserRecordMapperTest
{
    [Theory]
    [InlineData(GenderType.Female, "F")]
    [InlineData(GenderType.Male, "M")]
    [InlineData(GenderType.Other, "O")]
    public void MapsEachGenderToDbType(GenderType type, string expectedValue)
    {
        var record = (AValidUser() with {Gender = type}).AsRecord();
        record.Gender.Should().Be(expectedValue);
    }



    [Fact]
    public void MapsUserToRecord()
    {
        var user = AValidUser();
        
        user.AsRecord().Name.Should().Be(user.Name);
        user.AsRecord().Email.Should().Be(user.Email);
    }
    
    private static User AValidUser()
    {
        return new User("tony", "tony@hotmail.com", GenderType.Male);
    }
}