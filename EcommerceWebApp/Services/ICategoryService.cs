using Dapper;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<string?> GetCategoryByIdAsync(int id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly DapperContext _context;
        public CategoryService(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var query = "SELECT * FROM Categories";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Category>(query);
        }
        public async Task<string> GetCategoryByIdAsync(int id)
        {
            int? categoryId = null;
            var query = "SELECT Name FROM Categories WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<string>(query, new { Id = id }) ?? "Unknown";
        }
    }
}
