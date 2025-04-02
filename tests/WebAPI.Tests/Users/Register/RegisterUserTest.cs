using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
        response.RootElement.GetProperty("token").GetString().Should().StartWith("eyJ");
    }
}
