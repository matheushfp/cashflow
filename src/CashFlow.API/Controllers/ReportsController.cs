﻿using System.Net.Mime;
using CashFlow.Application.UseCases.Reports.Excel;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;
[Route("api/report")]
[ApiController]
public class ReportsController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateExpensesReportExcelUseCase useCase,
        [FromQuery] DateOnly month)
    {
        byte[] fileData = await useCase.Execute(month);

        if (fileData.Length > 0)
            return File(fileData, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }
}