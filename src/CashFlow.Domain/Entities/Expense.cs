﻿using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
    public ICollection<Tag> Tags { get; set; } = [];

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
