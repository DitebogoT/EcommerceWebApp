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
}
