using Dapper;
using EcommerceWebApp.Models;
using EcommerceWebApp.Data;
using System.Security.Cryptography;
using System.Text;

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
