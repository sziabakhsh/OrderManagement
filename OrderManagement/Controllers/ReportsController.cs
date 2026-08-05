using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ISsrsReportService _ssrsReportService;

        public ReportsController(ISsrsReportService ssrsReportService)
        {
            _ssrsReportService = ssrsReportService;
        }

        [HttpGet("customer-performance/export")]
        public async Task<IActionResult> ExportCustomerReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] string? region,
            [FromQuery] string format = "PDF",
            CancellationToken cancellationToken = default)
        {
            var fileBytes = await _ssrsReportService.GenerateCustomerPerformanceReportAsync(
                startDate, endDate, region, format, cancellationToken);

            string contentType = format.ToUpper() switch
            {
                "EXCEL" or "EXCELOPENXML" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "WORD" or "WORDOPENXML" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/pdf"
            };

            string fileName = $"Customer_Performance_Report_{DateTime.UtcNow:yyyyMMdd}.{format.ToLower()}";

            return File(fileBytes, contentType, fileName);
        }
    }
}
