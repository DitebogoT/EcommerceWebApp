using Dapper;
using EcommerceWebApp.Models;
using EcommerceWebApp.Data;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceWebApp.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}
