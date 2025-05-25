using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Expenses.Register;
public class RegisterExpenseTest : CashFlowClassFixture
{
    private const string REGISTER_EXPENSE_URI = "api/expenses";
    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = ExpenseRequestBuilder.Build();

        var result = await DoPost(requestUri: REGISTER_EXPENSE_URI, request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task EmptyTitleError(string culture)
    {
        var request = ExpenseRequestBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPost(requestUri: REGISTER_EXPENSE_URI, request: request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
