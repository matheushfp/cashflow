using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Users.Login;
public class UserLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string LOGIN_URI = "api/login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public UserLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _password = webApplicationFactory.GetPassword();
        _name = webApplicationFactory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new LoginRequest
        {
            Email = _email,
            Password = _password
        };

        var result = await _httpClient.PostAsJsonAsync(LOGIN_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("name").GetString().Should().Be(_name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace().And.StartWith("eyJ");
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task InvalidLoginError(string cultureInfo)
    {
        var request = LoginRequestBuilder.Build();

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

        var result = await _httpClient.PostAsJsonAsync(LOGIN_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVALID_CREDENTIALS", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
