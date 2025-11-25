using Dapper;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Services
{
    public interface IOrderService
    {
        Task<int> PlaceOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
    }
    public class OrderService : IOrderService
    {
        private readonly DapperContext _context;
        public OrderService(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> PlaceOrderAsync(Order order)
        {
            var query = @"
            INSERT INTO Orders (UserId, TotalAmount, OrderDate, Status)
            VALUES (@UserId, @TotalAmount, @OrderDate, @Status);
            SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            order.OrderDate = DateTime.UtcNow;
            order.Status = "Pending";
            return await connection.ExecuteScalarAsync<int>(query, order);
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM Orders WHERE UserId = @UserId ORDER BY OrderDate DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Order>(query, new { UserId = userId });
        }
    }
}
