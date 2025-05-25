using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Expenses.Update;

public class UpdateExpenseTest : CashFlowClassFixture
{
    private const string UPDATE_EXPENSE_URI = "api/expenses";
    
    private readonly string _token;
    private readonly Guid _expenseId;
    
    public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = ExpenseRequestBuilder.Build();
        
        var result = await DoPut(requestUri: $"{UPDATE_EXPENSE_URI}/{_expenseId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task EmptyTitleError(string culture)
    {
        var request = ExpenseRequestBuilder.Build(); 
        request.Title = string.Empty;

        var result = await DoPut(requestUri: $"{UPDATE_EXPENSE_URI}/{_expenseId}", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ExpenseNotFoundError(string culture)
    {
        var request = ExpenseRequestBuilder.Build();

        var result = await DoPut(requestUri: $"{UPDATE_EXPENSE_URI}/{Guid.NewGuid()}", request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}