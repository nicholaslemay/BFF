using BFF.Support.Database;
using static BFF.Users.GenderType;

namespace BFF.Users;

public interface IUserRepository
{
    Task<int> AddUser(User user);
}

public class UserRepository : IUserRepository
{
    private readonly BffDb _db;

    public UserRepository(BffDb db) => _db = db;

    public async Task<int>  AddUser(User u)
    {
        var newRecord = u.AsRecord();
        _db.Users.Add(newRecord);
        await _db.SaveChangesAsync();

        return newRecord.Id;
    }
}

public static class UserRecordMapper
{
    public static UserRecord AsRecord(this User u) =>
        new()
        {
            Email = u.Email,
            Name = u.Name,
            Gender = u.Gender switch
            {
                Female => "F",
                Male => "M",
                Other => "O",
            }
        };
}