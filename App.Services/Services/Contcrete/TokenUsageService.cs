using App.Data;
using System.Security.Cryptography;
using System.Text;

namespace App.Services.Services.Contcrete
{
    public class TokenUsageService
    {
        public readonly AppDbContext _dbContext;

        public TokenUsageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }



    }
}
