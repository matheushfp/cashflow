using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Users.Update;
public interface IUpdateUserUseCase
{
    Task Execute(UpdateUserRequest request);
}
