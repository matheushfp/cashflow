using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Login;

public interface IUserLoginUseCase
{
    Task<RegisterUserResponse> Execute(LoginRequest request);
}
