using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;

public interface IUsersReadOnlyRepository
{
    Task<bool> UserWithThisEmailAlreadyExists(string email);
    Task<User?> FindUserByEmail(string email);
}
