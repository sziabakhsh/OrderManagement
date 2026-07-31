
namespace Application.DTOs
{
    public class CustomerReportDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int TotalOrdersCount { get; set; }
        public decimal TotalSpentAmount { get; set; }
        public int TotalItemsPurchased { get; set; }
        public string TopCategoryName { get; set; } = string.Empty;
    }
}
