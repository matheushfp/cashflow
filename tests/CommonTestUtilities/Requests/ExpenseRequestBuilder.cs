﻿using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class ExpenseRequestBuilder
{
    public static ExpenseRequest Build()
    {
        return new Faker<ExpenseRequest>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Tags, faker => faker.Make(1, () => faker.PickRandom<Tag>()));
    }
}
