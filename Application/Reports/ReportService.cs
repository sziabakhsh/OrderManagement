using Application.Common.Interfaces;
using Application.DTOs;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Application.Reports
{
    public class ReportService : IReportService
    {
        private readonly IAppDbContext _dbContext;

        public ReportService(IAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<CustomerReportDto>> GetCustomerPerformanceReportAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            // ۱. محاسبه تاریخ پایه قبل از شروع کوئری
            var ninetyDaysAgo = DateTime.UtcNow.AddDays(-90);

            // ۲. کوئری بهینه با یک‌بار فیلتر کردن سفارشات و بدون Join دستی
            var query = _dbContext.Customers
                .AsNoTracking() // عدم Tracking برای بالا بردن سرعت گزارش‌گیری Read-Only
                .Select(c => new
                {
                    Customer = c,
                    // فیلتر کردن سفارشات ۹۰ روز گذشته مشتری در یک متغیر موقت
                    RecentOrders = c.Orders.Where(o => o.Status == OrderStatus.Completed && o.OrderDate >= ninetyDaysAgo)
                })
                .Select(x => new CustomerReportDto
                {
                    CustomerId = x.Customer.Id,
                    CustomerName = x.Customer.Name,
                    Region = x.Customer.Region,

                    // تعداد کل سفارشات
                    TotalOrdersCount = x.RecentOrders.Count(),

                    // کل مبلغ پرداختی واقعی
                    TotalSpentAmount = x.RecentOrders
                        .SelectMany(o => o.OrderItems)
                        .Sum(oi => (oi.Quantity * oi.UnitPrice) - oi.Discount),

                    // تعداد کل کالاهای خریده شده
                    TotalItemsPurchased = x.RecentOrders
                        .SelectMany(o => o.OrderItems)
                        .Sum(oi => oi.Quantity),

                    // دسته‌بندی با بیشترین تعداد خرید
                    TopCategoryName = x.RecentOrders
                        .SelectMany(o => o.OrderItems)
                        .GroupBy(oi => oi.Product.CategoryName)
                        .OrderByDescending(g => g.Sum(oi => oi.Quantity))
                        .Select(g => g.Key)
                        .FirstOrDefault()??""
                });

            return await PaginatedResult<CustomerReportDto>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
        }
    }
}
