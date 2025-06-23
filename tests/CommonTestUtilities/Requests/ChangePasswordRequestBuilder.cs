using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class ChangePasswordRequestBuilder
{
    public static ChangePasswordRequest Build()
    {
        return new Faker<ChangePasswordRequest>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}