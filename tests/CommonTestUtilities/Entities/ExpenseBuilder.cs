using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class ExpenseBuilder
{
    public static List<Expense> Collection(User user, uint count = 2)
    {
        var expenses = new List<Expense>();

        if (count == 0)
            count = 1;

        for(int i = 0; i < count; i++)
        {
            var expense = ExpenseBuilder.Build(user);

            expenses.Add(expense);
        }

        return expenses;
    }

    public static Expense Build(User user)
    {
        return new Faker<Expense>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(1, 1000))
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.UserId, _ => user.Id);
    }
}
