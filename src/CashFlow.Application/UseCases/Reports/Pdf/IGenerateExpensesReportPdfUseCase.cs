namespace CashFlow.Application.UseCases.Reports.Pdf;
public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
