using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;
using LoginRequest = CashFlow.Communication.Requests.LoginRequest;

namespace WebAPI.Tests.Users.ChangePassword;

public class ChangePasswordTest : CashFlowClassFixture
{
    private const string CHANGE_PASSWORD_URI = "api/users/change-password";

    private readonly string _token;
    private readonly string _password;
    private readonly string _email;
    
    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
        _email = webApplicationFactory.UserTeamMember.GetEmail();
    }
    
    [Fact]
    public async Task Success()
    {
        var request = ChangePasswordRequestBuilder.Build();
        request.Password = _password;

        var response = await DoPut(CHANGE_PASSWORD_URI, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new LoginRequest
        {
            Email = _email,
            Password = _password,
        };

        response = await DoPost("api/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost("api/login", loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task CurrentPasswordMismatchError(string culture)
    {
        var request = ChangePasswordRequestBuilder.Build();

        var result = await DoPut(CHANGE_PASSWORD_URI, request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("CURRENT_PASSWORD_MISMATCH", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}