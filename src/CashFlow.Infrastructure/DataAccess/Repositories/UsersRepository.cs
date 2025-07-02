using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UsersRepository : IUsersReadOnlyRepository, IUsersWriteOnlyRepository, IUsersUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public UsersRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task Delete(User user)
    {
        var userToDelete = await _dbContext.Users.FindAsync(user.Id);
        _dbContext.Users.Remove(userToDelete!);
    }

    public async Task<User?> FindUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetById(Guid id)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }

    public async Task<bool> UserWithThisEmailAlreadyExists(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }
}
