namespace EcommerceWebApp.ViewModels
{
    public class CartItemViewModels
    {
        public int ProductID { get; set; }
        public int ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 0;
        public decimal TotalPrice { get; set; }
    }
}
