using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportMessages.CASH,
            PaymentType.CreditCard => ResourceReportMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportMessages.DEBIT_CARD,
            PaymentType.ElectronicTransfer => ResourceReportMessages.ELECTRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}
