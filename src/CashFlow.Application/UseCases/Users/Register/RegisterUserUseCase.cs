using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;

    public RegisterUserUseCase(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<RegisterUserResponse> Execute(RegisterUserRequest request)
    {
        Validate(request);

        var user = _mapper.Map<User>(request);

        return new RegisterUserResponse
        {
            Name = user.Name
        };
    }

    private void Validate(RegisterUserRequest request)
    {
        var result = new RegisterUserValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ValidationErrorException(errorMessages);
        }
    }
}
