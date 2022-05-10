using Microsoft.EntityFrameworkCore;

namespace BFF.Support.Database;

public class BffDb : DbContext
{
    public BffDb(DbContextOptions<BffDb> options) : base(options) { }
    
    public DbSet<UserRecord> Users => Set<UserRecord>(); 
}

public class UserRecord 
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
     public string Name { get; set; } = null!;
     //public string Status { get; set; } = null!;
     public string Gender { get; set; } = null!;
}