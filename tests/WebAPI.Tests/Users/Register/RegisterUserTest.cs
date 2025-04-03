using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebAPI.Tests.Users.Register;
public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string REGISTER_USER_URI = "api/users";
    private readonly HttpClient _httpClient;

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RegisterUserRequestBuilder.Build();

        var result = await _httpClient.PostAsJsonAsync(REGISTER_USER_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty().And.StartWith("eyJ");
    }

    [Fact]
    public async Task EmptyNameError()
    {
        var request = RegisterUserRequestBuilder.Build();
        request.Name = string.Empty;

        var result = await _httpClient.PostAsJsonAsync(REGISTER_USER_URI, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);

        var errors = response.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.EMPTY_NAME));
    }
}
