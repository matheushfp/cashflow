using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RegisterExpenseRequest, Expense>();
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, RegisterExpenseResponse>();
        CreateMap<Expense, ExpenseShortResponse>();
        CreateMap<Expense, ExpenseResponse>();
    }
}
