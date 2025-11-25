//using EcommerceWebApp.ViewModels;

//namespace EcommerceWebApp.Models
//{
//    public class Cart
//    {
//        public Guid Id { get; set; }
//        public string CartItem { get; set; }
//        public string CartTitle { get; set; }
//        public decimal Price { get; set; }
//        public decimal TotalPrice { get; set; }
//        public decimal CartTotal { get; set; } = 0;
//       // public ICollection<CartItemViewModels> CartItems { get; set; } = new List<CartItemViewModels>();
//        public List<CartItem> Items { get; set; } = new();
//        public decimal Total => Items.Sum(static i => i.Price * i.Quantity);
//        public int ItemCount => Items.Sum(i => i.Quantity);
//    }
//    public class CartItem
//    {
//        public int ProductID { get; set; }
//        public string Product { get; set; }
//        public int Quantity { get; set; } = 0;
//        public decimal Price { get; set; }
//        public decimal TotalPrice { get; set; }
//    }
//}
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Models;

public class CartItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }

    // Navigation property
    public Product? Product { get; set; }
}

public class Cart
{
    public List<CartItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Product!.Price * i.Quantity);
    public int ItemCount => Items.Sum(i => i.Quantity);
    public int ProductID { get; set; }
    public decimal CartTotal { get; set; } = 0;
    //public string Product { get; set; }
    //       public int Quantity { get; set; } = 0;
    //       public decimal Price { get; set; }
           public decimal TotalPrice { get; set; }
}

