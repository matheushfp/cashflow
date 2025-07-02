using System.Net;
using FluentAssertions;

namespace WebAPI.Tests.Users.Delete;

public class DeleteUserTest : CashFlowClassFixture
{
    private const string DELETE_USER_URI = "api/users";
    
    private readonly string _token;
    
    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(DELETE_USER_URI, _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}