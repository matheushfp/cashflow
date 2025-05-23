using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Users.Login;
public class UserLoginTest : CashFlowClassFixture
{
    private const string LOGIN_URI = "api/login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public UserLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.UserTeamMember.GetEmail();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
        _name = webApplicationFactory.UserTeamMember.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new LoginRequest
        {
            Email = _email,
            Password = _password
        };

        var result = await DoPost(LOGIN_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("name").GetString().Should().Be(_name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace().And.StartWith("eyJ");
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task InvalidLoginError(string culture)
    {
        var request = LoginRequestBuilder.Build();

        var result = await DoPost(requestUri: LOGIN_URI, request: request, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("INVALID_CREDENTIALS", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
