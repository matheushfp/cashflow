using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class LoginRequestBuilder
{
    public static LoginRequest Build()
    {
        return new Faker<LoginRequest>()
            .RuleFor(user => user.Email, faker => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "*Aa1"));
    }
}
