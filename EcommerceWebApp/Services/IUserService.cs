using Dapper;
using EcommerceWebApp.Models;
using EcommerceWebApp.Data;
using System.Security.Cryptography;
using System.Text;

namespace EcommerceWebApp.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> ValidatePasswordAsync(string email, string password);
    }
    public class UserService : IUserService
    {
        private readonly DapperContext _context;

        public UserService(DapperContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var query = "SELECT * FROM Users WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Id = id });
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM Users WHERE Email = @Email";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<int> CreateUserAsync(User user, string password)
        {
            var query = @"
            INSERT INTO Users (Email, PasswordHash, FirstName, LastName, Address, PhoneNumber, CreatedAt, IsAdmin)
            VALUES (@Email, @PasswordHash, @FirstName, @LastName, @Address, @PhoneNumber, @CreatedAt, @IsAdmin);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = _context.CreateConnection();
            user.PasswordHash = HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            return await connection.ExecuteScalarAsync<int>(query, user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var query = @"
            UPDATE Users 
            SET Email = @Email,
                FirstName = @FirstName,
                LastName = @LastName,
                Address = @Address,
                PhoneNumber = @PhoneNumber
            WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, user);
            return affectedRows > 0;
        }

        public async Task<bool> ValidatePasswordAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            var hashedPassword = HashPassword(password);
            return user.PasswordHash == hashedPassword;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
