using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RegisterUserRequestBuilder
{
    public static RegisterUserRequest Build()
    {
        return new Faker<RegisterUserRequest>()
            .RuleFor(r => r.Name, faker => faker.Person.FirstName)
            .RuleFor(r => r.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(r => r.Password, faker => faker.Internet.Password(prefix: "*Aa1"));
    }
}
