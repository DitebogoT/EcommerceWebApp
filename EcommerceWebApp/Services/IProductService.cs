using Dapper;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> CreateProductAsync(Product product);
        Task<IEnumerable<Product>> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<Product?> GetProductsByCategoryAsync(int categoryId);
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
        public async Task<Product?> GetProductsByCategoryAsync(int categoryId)
        {
            var query = "SELECT * FROM Products WHERE CategoryId = @Category";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Product>(query, new { Category = categoryId });
        }
        public async Task<IEnumerable<Product>> CreateProductAsync(Product product)
        {
            var query = "INSERT INTO Products (Name, Description, Price, CategoryId) VALUES (@Name, @Description, @Price, @CategoryId); SELECT * FROM Products;";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, product);
            return await GetAllProductsAsync();
        }
        public async Task<IEnumerable<Product>> UpdateProductAsync(Product product)
        {
            var query = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, CategoryId = @CategoryId WHERE Id = @Id; SELECT * FROM Products;";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, product);
            return await GetAllProductsAsync();
        }
        public async Task DeleteProductAsync(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
