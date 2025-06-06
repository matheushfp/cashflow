﻿using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUsersReadOnlyRepository _usersReadOnlyRepository;
    private readonly IUsersUpdateOnlyRepository _usersUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUsersReadOnlyRepository usersReadOnlyRepository,
        IUsersUpdateOnlyRepository usersUpdateOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _usersReadOnlyRepository = usersReadOnlyRepository;
        _usersUpdateOnlyRepository = usersUpdateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(UpdateUserRequest request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await _usersUpdateOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _usersUpdateOnlyRepository.Update(user);
        await _unitOfWork.Commit();
    }

    private async Task Validate(UpdateUserRequest request, string currentEmail)
    {
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userAlreadyExists = await _usersReadOnlyRepository.UserWithThisEmailAlreadyExists(request.Email);

            if (userAlreadyExists)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ValidationErrorException(errorMessages);
        }
    }
}
