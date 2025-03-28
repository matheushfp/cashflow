using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UsersReadOnlyRepositoryBuilder
{
    private readonly Mock<IUsersReadOnlyRepository> _repository;

    public UsersReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUsersReadOnlyRepository>();
    }

    public void UserWithThisEmailAlreadyExists(string email)
    {
        _repository.Setup(repository => repository.UserWithThisEmailAlreadyExists(email)).ReturnsAsync(true);
    }

    public IUsersReadOnlyRepository Build() => _repository.Object;
}
