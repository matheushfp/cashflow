using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Users.Login;

public class UserLoginUseCase : IUserLoginUseCase
{
    private readonly IUsersReadOnlyRepository _repository;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public UserLoginUseCase(
        IUsersReadOnlyRepository repository,
        IPasswordEncrypter passwordEncrypter,
        IAccessTokenGenerator accessTokenGenerator
        )
    {
        _repository = repository;
        _passwordEncrypter = passwordEncrypter;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<RegisterUserResponse> Execute(LoginRequest request)
    {
        var user = await _repository.FindUserByEmail(request.Email);

        if (user is null)
            throw new InvalidCredentialsException();

        var passwordMatch = _passwordEncrypter.Verify(request.Password, user.Password);

        if(!passwordMatch)
            throw new InvalidCredentialsException();

        return new RegisterUserResponse
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}
