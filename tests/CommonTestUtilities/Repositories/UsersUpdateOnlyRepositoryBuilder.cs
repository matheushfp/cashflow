using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UsersUpdateOnlyRepositoryBuilder
{
    public static IUsersUpdateOnlyRepository Build(User user)
    {
        var mock = new Mock<IUsersUpdateOnlyRepository>();

        mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return mock.Object;
    }
}