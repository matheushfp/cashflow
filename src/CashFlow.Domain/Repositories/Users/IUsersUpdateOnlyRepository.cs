using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;
public interface IUsersUpdateOnlyRepository
{
    Task<User> GetById(Guid id);
    void Update(User user);
}
