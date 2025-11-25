using Dapper;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;
        public ProductService(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var query = "SELECT * FROM Products";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query);
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var query = "SELECT * FROM Products WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Id = id });
        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            var query = "SELECT * FROM Products WHERE Name LIKE @SearchTerm OR Description LIKE @SearchTerm";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(query, new { SearchTerm = $"%{searchTerm}%" });
        }
    }
}
