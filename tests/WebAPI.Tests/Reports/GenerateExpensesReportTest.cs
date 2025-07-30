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
        var month = _expenseDate.ToString("yyyy-MM");
        var result = await DoGet($"{GENERATE_REPORT_URI}/pdf?month={month}", _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }
    
    [Fact]
    public async Task ExcelSuccess()
    {
        var month = _expenseDate.ToString("yyyy-MM");
        var result = await DoGet($"{GENERATE_REPORT_URI}/excel?month={month}",  _adminToken);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task PdfForbiddenError()
    {
        var month = _expenseDate.ToString("yyyy-MM");
        var result = await DoGet($"{GENERATE_REPORT_URI}/pdf?month={month}",  _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ExcelForbiddenError()
    {
        var month = _expenseDate.ToString("yyyy-MM");
        var result = await DoGet($"{GENERATE_REPORT_URI}/excel?month={month}",  _teamMemberToken);

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}