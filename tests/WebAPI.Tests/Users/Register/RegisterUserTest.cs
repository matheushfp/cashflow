using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Users.Register;
public class RegisterUserTest : CashFlowClassFixture
{
    private const string REGISTER_USER_URI = "api/users";

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var result = await DoPost(REGISTER_USER_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty().And.StartWith("eyJ");
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task EmptyNameError(string culture)
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(requestUri: REGISTER_USER_URI, request: request, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMPTY_NAME", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
