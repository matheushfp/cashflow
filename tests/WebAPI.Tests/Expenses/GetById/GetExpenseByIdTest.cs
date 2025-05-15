using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Expenses.GetById;
public class GetExpenseByIdTest : CashFlowClassFixture
{
    private readonly string GET_EXPENSE_BY_ID_URI = "api/expenses";
    private readonly string _token;
    private readonly Guid _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
        _expenseId = webApplicationFactory.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{GET_EXPENSE_BY_ID_URI}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("id").GetGuid().Should().Be(_expenseId);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ExpenseNotFoundError(string culture)
    {
        var result = await DoGet(requestUri: $"{GET_EXPENSE_BY_ID_URI}/{Guid.NewGuid()}", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
