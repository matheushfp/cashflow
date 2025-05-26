using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Security.Cryptography;

namespace CommonTestUtilities.Entities;
public class UserBuilder
{
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();

        return new Faker<User>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.Name, faker => faker.Person.FirstName)
            .RuleFor(r => r.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(r => r.Password, (_, user) => passwordEncrypter.Encrypt(user.Password))
            .RuleFor(r => r.Role, _ => role);
    }
}
