using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Expenses.Register;
public class RegisterExpenseTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string REGISTER_EXPENSE_URI = "api/expenses";
    private readonly HttpClient _httpClient;
    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _token = webApplicationFactory.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RegisterExpenseRequestBuilder.Build();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var result = await _httpClient.PostAsJsonAsync(REGISTER_EXPENSE_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task EmptyTitleError(string cultureInfo)
    {
        var request = RegisterExpenseRequestBuilder.Build();
        request.Title = string.Empty;

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

        var result = await _httpClient.PostAsJsonAsync(REGISTER_EXPENSE_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
