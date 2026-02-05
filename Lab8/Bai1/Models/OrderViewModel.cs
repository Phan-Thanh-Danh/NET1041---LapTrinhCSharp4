namespace Bai1.Models
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string DishName { get; set; } = string.Empty;
    }

    public class OrderComparisonViewModel
    {
        public List<Order> EagerLoadingResult { get; set; } = new();
        public List<OrderDetailViewModel> RawSqlResult { get; set; } = new();
    }
}
