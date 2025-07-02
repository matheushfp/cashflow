using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;

public interface IUsersWriteOnlyRepository
{
    Task Add(User user);
    Task Delete(User user);
}
