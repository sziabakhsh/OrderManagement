using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class SsrsReportService : ISsrsReportService
{
    private readonly HttpClient _httpClient;
    private readonly string _reportServerUrl;

    public SsrsReportService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        // آدرس SSRS Report Server از appsettings خوانده می‌شود
        _reportServerUrl = configuration["SsrsSettings:ReportServerUrl"]
            ?? "http://localhost/ReportServer";
    }

    public async Task<byte[]> GenerateCustomerPerformanceReportAsync(
        DateTime startDate,
        DateTime endDate,
        string? region = null,
        string format = "PDF",
        CancellationToken cancellationToken = default)
    {
        // ساخت URL فراخوانی مستقیم SSRS Report Execution
        // فرمت URL استاندارد SSRS:
        // http://<Server>/ReportServer?/ReportPath&Param1=Value1&rs:Format=PDF

        var formattedStartDate = startDate.ToString("yyyy-MM-dd");
        var formattedEndDate = endDate.ToString("yyyy-MM-dd");

        var requestUrl = $"{_reportServerUrl}?/OrderReports/CustomerPerformance" +
                         $"&StartDate={formattedStartDate}" +
                         $"&EndDate={formattedEndDate}" +
                         $"&rs:Format={format}";

        if (!string.IsNullOrEmpty(region))
        {
            requestUrl += $"&Region={Uri.EscapeDataString(region)}";
        }

        // send the HTTP request to SSRS Report Server
        var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}