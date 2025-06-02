using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebAPI.Tests.Users.Profile;

public class GetUserProfileTest : CashFlowClassFixture
{
    private const string GET_USER_PROFILE_URI = "api/users";

    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;
    
    public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _userName = webApplicationFactory.UserTeamMember.GetName();
        _userEmail = webApplicationFactory.UserTeamMember.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(GET_USER_PROFILE_URI, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseBody = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(responseBody);
        
        response.RootElement.GetProperty("name").GetString().Should().Be(_userName);
        response.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
    }
}