using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BFF.Tests.Support;
using BFF.Tests.Support.Database;
using BFF.Users;
using FluentAssertions;
using Xunit;
using static BFF.Users.GenderType;

namespace BFF.Tests.Users;

public class UserRepositoryTest : DatabaseTest
{
    private readonly UserRepository _userRepository;

    public UserRepositoryTest(DatabaseTestFixture fixture) : base(fixture)
    {
        _userRepository = new UserRepository(_db);
    }

    [Fact]
    public async Task CanStoreAUserInDb()
    {
        var user = new User{Email = "bobby@hotmail.com", Gender = Male, Name = "bobby"};  
        _userRepository.AddUser(user);

        var savedUser = _db.Users.First();
        savedUser.Email.Should().Be(user.Email);
        savedUser.Name.Should().Be(user.Name);
        savedUser.Gender.Should().Be("M");
    }
    
    [Fact]
    public async Task AllUsersHaveAUniqueId()
    {
        var user = new User{Email = "tony@hotmail.com", Gender = Male, Name = "tony"};  
        
        List<int> ids = new();
        30.Times(async ()=> ids.Add(await _userRepository.AddUser(user)));

        ids.Should().HaveCount(30).And.OnlyHaveUniqueItems();
    }
}