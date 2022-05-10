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
        var record = new User{ Gender = type }.AsRecord();
        record.Gender.Should().Be(expectedValue);
    }
    
    [Fact]
    public void MapsUserToRecord()
    {
        var user = new User { Name = "tony", Email = "tony@hotmail.com" };
        
        user.AsRecord().Name.Should().Be(user.Name);
        user.AsRecord().Email.Should().Be(user.Email);
    }
}