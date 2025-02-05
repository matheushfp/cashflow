﻿namespace CashFlow.Communication.Responses;

public class ExpenseShortResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
