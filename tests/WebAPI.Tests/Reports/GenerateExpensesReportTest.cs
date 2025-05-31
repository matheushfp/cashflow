using System.Net;
using System.Net.Mime;
using FluentAssertions;

namespace WebAPI.Tests.Reports;

public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string GENERATE_REPORT_URI = "api/report";

    private readonly string _adminToken;
    private readonly DateTime _expenseDate;
    private readonly string _teamMemberToken;
    
    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.UserAdmin.GetToken();
        _expenseDate = webApplicationFactory.ExpenseAdmin.GetDate();
        _teamMemberToken = webApplicationFactory.UserTeamMember.GetToken();
    }

    [Fact]
    public async Task PdfSuccess()
    {
        var result = await DoGet($"{GENERATE_REPORT_URI}/pdf?month={_expenseDate:Y}",  _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }
    
    [Fact]
    public async Task ExcelSuccess()
    {
        var result = await DoGet($"{GENERATE_REPORT_URI}/excel?month={_expenseDate:Y}",  _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task PdfForbiddenError()
    {
        var result = await DoGet($"{GENERATE_REPORT_URI}/pdf?month={_expenseDate:Y}",  _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ExcelForbiddenError()
    {
        var result = await DoGet($"{GENERATE_REPORT_URI}/excel?month={_expenseDate:Y}",  _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}