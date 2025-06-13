using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class UpdateUserRequestBuilder
{
    public static UpdateUserRequest Build()
    {
        return new Faker<UpdateUserRequest>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}