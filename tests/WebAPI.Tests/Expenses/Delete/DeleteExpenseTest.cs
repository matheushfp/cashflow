using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Expenses.Delete;
public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string DELETE_EXPENSE_URI = "api/expenses";

    private readonly string _token;
    private readonly Guid _expenseId;

    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{DELETE_EXPENSE_URI}/{_expenseId}", token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);


        result = await DoGet(requestUri: $"{DELETE_EXPENSE_URI}/{_expenseId}", token: _token);
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ExpenseNotFoundError(string culture)
    {
        var result = await DoDelete(requestUri: $"{DELETE_EXPENSE_URI}/{Guid.NewGuid()}", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}