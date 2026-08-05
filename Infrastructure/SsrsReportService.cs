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
        // Ssrs setting is read from appsettings.json or environment variables. If not found, default to localhost.
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
        var formattedStartDate = startDate.ToString("yyyy-MM-dd");
        var formattedEndDate = endDate.ToString("yyyy-MM-dd");

        // The full path to the report in SSRS
        var requestUrl = $"{_reportServerUrl}?/ReportService/CustomerPerformanceReport" +
                         $"&StartDate={formattedStartDate}" +
                         $"&EndDate={formattedEndDate}" +
                         $"&rs:Format={format}";

        if (!string.IsNullOrEmpty(region))
        {
            requestUrl += $"&Region={Uri.EscapeDataString(region)}";
        }

        var response = await _httpClient.GetAsync(requestUrl, cancellationToken);

        // If the response is not successful, log the exact SSRS error message
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"SSRS Server returned status {response.StatusCode}. Details: {errorContent}");
        }

        //if (!response.IsSuccessStatusCode)
        //{
        //    // If the SSRS server is not available, generate a sample file for testing
        //    return System.Text.Encoding.UTF8.GetBytes("%PDF-1.4 Mock SSRS Report Content for Testing");
        //}
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }
}