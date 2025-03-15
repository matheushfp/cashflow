using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IUsersReadOnlyRepository _usersReadOnlyRepository;

    public RegisterUserUseCase(
        IMapper mapper, 
        IPasswordEncrypter passwordEncrypter, 
        IUsersReadOnlyRepository usersReadOnlyRepository)
    {
        _mapper = mapper;
        _passwordEncrypter = passwordEncrypter;
        _usersReadOnlyRepository = usersReadOnlyRepository;
    }

    public async Task<RegisterUserResponse> Execute(RegisterUserRequest request)
    {
        await Validate(request);

        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncrypter.Encrypt(request.Password);

        return new RegisterUserResponse
        {
            Name = user.Name
        };
    }

    private async Task Validate(RegisterUserRequest request)
    {
        var result = new RegisterUserValidator().Validate(request);

        var emailAlreadyExists = await _usersReadOnlyRepository.UserWithThisEmailAlreadyExists(request.Email);

        if (emailAlreadyExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ValidationErrorException(errorMessages);
        }
    }
}
