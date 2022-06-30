using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BFF.Support.Database;

public class BffDb : DbContext
{
    public BffDb(DbContextOptions<BffDb> options) : base(options) { }
    
    public DbSet<UserRecord> Users => Set<UserRecord>(); 
}

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync(IDbContextTransaction transaction);
}
public class UnitOfWork : IUnitOfWork
{
    private readonly BffDb _database;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(BffDb Database)
    {
        _database = Database;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null) return null;

        _currentTransaction = await _database.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await _database.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }
}



public class UserRecord 
{
    public int Id { get; set; }
    public string Email { get; init; } = null!;
     public string Name { get; init; } = null!;
     //public string Status { get; set; } = null!;
     public string Gender { get; init; } = null!;
}