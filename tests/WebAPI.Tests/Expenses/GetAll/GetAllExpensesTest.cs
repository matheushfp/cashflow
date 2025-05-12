using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebAPI.Tests.Expenses.GetAll;
public class GetAllExpensesTest : CashFlowClassFixture
{
    private const string GET_ALL_EXPENSES_URI = "api/expenses";
    private readonly string _token;


    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: GET_ALL_EXPENSES_URI, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}
