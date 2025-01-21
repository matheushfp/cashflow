using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Register([FromBody] RegisterExpenseRequest request)
    {
        try
        {
            var useCase = new RegisterExpenseUseCase();
            var response = useCase.Execute(request);

            return Created(string.Empty, response);
        }
        catch (ValidationErrorException ex)
        {
            var errorsResponse = new ErrorResponse(ex.Errors);

            return BadRequest(errorsResponse);
        }
        catch
        {
            var errorsResponse = new ErrorResponse("Unknown Error");

            return StatusCode(StatusCodes.Status500InternalServerError, errorsResponse);
        }
    }
}
