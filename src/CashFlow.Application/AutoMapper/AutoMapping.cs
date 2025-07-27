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
        CreateMap<RegisterUserRequest, User>()
            .ForMember(dest => dest.Password, options => options.Ignore());

        CreateMap<ExpenseRequest, Expense>()
            .ForMember(dest => dest.Tags, options => options.MapFrom(source => source.Tags.Distinct()));
        
        CreateMap<Communication.Enums.Tag, Tag>()
            .ForMember(dest => dest.Value, options => options.MapFrom(source => source));
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ExpenseResponse>()
            .ForMember(dest => dest.Tags, options => options.MapFrom(source => source.Tags.Select(tag => tag.Value)));
        CreateMap<Expense, RegisterExpenseResponse>();
        CreateMap<Expense, ExpenseShortResponse>();
        CreateMap<User, UserProfileResponse>();
    }
}
