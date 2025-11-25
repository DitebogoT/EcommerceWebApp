using Dapper;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Services
{
    public interface ICartService
    {
        Task<int> AddToCartAsync(Cart cartItem);
        Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(int userId);
        Task<bool> RemoveFromCartAsync(int cartItemId);

    }
    public class CartService : ICartService
    {
        private readonly DapperContext _context;
        public CartService(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddToCartAsync(Cart cartItem)
        {
            var query = @"
            INSERT INTO CartItems (UserId, ProductId, Quantity)
            VALUES (@UserId, @ProductId, @Quantity);
            SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, cartItem);
        }
        public async Task<IEnumerable<Cart>> GetCartItemsByUserIdAsync(int userId)
        {
            var query = "SELECT * FROM CartItems WHERE UserId = @UserId";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Cart>(query, new { UserId = userId });
        }
        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            var query = "DELETE FROM CartItems WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var result = await connection.ExecuteAsync(query, new { Id = cartItemId });
            return result > 0;
        }
    }
}
