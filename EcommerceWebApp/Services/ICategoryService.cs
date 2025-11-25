using Dapper;
using EcommerceWebApp.Data;
using EcommerceWebApp.Models;

namespace EcommerceWebApp.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
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
    }
}
