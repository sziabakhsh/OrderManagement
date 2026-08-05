using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISsrsReportService
    {
        Task<byte[]> GenerateCustomerPerformanceReportAsync(
        DateTime startDate,
        DateTime endDate,
        string? region = null,
        string format = "PDF",
        CancellationToken cancellationToken = default);
    }
}
