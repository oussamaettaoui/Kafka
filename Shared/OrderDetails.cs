namespace Shared
{
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
