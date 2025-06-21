using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUsersUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser, 
        IUsersUpdateOnlyRepository repository, 
        IUnitOfWork unitOfWork, 
        IPasswordEncrypter passwordEncrypter)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncrypter = passwordEncrypter;
    }
    
    public async Task Execute(ChangePasswordRequest request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);
        user.Password = _passwordEncrypter.Encrypt(request.NewPassword);

        _repository.Update(user);
        await _unitOfWork.Commit();
    }
    
    private void Validate(ChangePasswordRequest request, User loggedUser)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordMatch = _passwordEncrypter.Verify(request.Password, loggedUser.Password);

        if (!passwordMatch)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.CURRENT_PASSWORD_MISMATCH));

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationErrorException(errors);
        }
}
}