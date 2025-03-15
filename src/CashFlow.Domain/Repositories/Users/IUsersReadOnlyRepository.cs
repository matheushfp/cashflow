namespace CashFlow.Domain.Repositories.Users;

public interface IUsersReadOnlyRepository
{
    Task<bool> UserWithThisEmailAlreadyExists(string email);
}
