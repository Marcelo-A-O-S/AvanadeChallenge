using System.Security.Cryptography;
using AuthService.Domain.Enums;
using System.Text;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        public void  createPasswordHash(string password)
        {
            using (var hmac = new HMACSHA256())
            {
                var bufferSalt = hmac.Key;
                this.PasswordSalt = Convert.ToBase64String(bufferSalt);
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                this.PasswordHash = Convert.ToBase64String(hash);
            }
        }
        public async Task<bool> verifyPassword(string password)
        {
            var key = Convert.FromBase64String(this.PasswordSalt);
            using (var hmac = new HMACSHA256(key))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = Convert.FromBase64String(this.PasswordHash);
                return computedHash.SequenceEqual(hash);
            }
        }
        public async Task SetPasswordHash(string passwordHash)
        {
            this.PasswordHash = passwordHash;
        }
        public async Task SetPasswordSalt(string passwordSalt)
        {
            this.PasswordSalt = passwordSalt;
        }
    }
}