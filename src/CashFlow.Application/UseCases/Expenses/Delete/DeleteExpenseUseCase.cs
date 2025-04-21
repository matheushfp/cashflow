
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _readOnlyRepository;
    private readonly IExpensesWriteOnlyRepository _writeOnlyrepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseUseCase(
        IExpensesReadOnlyRepository readOnlyRepository,
        IExpensesWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork,
        ILoggedUser loggedUser)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyrepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }

    public async Task Execute(Guid id)
    {
        var loggedUser = await _loggedUser.Get();

        var expense = await _readOnlyRepository.GetById(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _writeOnlyrepository.Delete(id);
        
        await _unitOfWork.Commit();
    }
}
