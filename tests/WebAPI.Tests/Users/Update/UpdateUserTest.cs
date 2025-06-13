using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Users.Update;

public class UpdateUserTest : CashFlowClassFixture
{
    private const string UPDATE_USER_URI = "api/users";
    private readonly string _token;
    
    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = UpdateUserRequestBuilder.Build();
        
        var response = await DoPut(UPDATE_USER_URI, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task EmptyNameError(string culture)
    {
        var request = UpdateUserRequestBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(UPDATE_USER_URI, request, token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMPTY_NAME", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}