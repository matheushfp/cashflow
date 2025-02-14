using System.Net.Mime;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromQuery] DateOnly month)
    {
        byte[] fileData = new byte[1];

        if (fileData.Length > 0 )
            return File(fileData, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }
}