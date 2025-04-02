using System.Net;
using System.Net.Http.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

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
    }
}
