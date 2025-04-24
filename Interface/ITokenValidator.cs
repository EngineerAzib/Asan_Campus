using Asan_Campus.Model;
using Microsoft.EntityFrameworkCore;

namespace Asan_Campus.Interface
{
    public interface ITokenValidator
    {
        Task<bool> IsTokenRevokedAsync(string token);
    }

    public class TokenValidator : ITokenValidator
    {
        private readonly ApplicationDbContext _context;

        public TokenValidator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTokenRevokedAsync(string token)
        {
            return await _context.RevokedTokens.AnyAsync(rt => rt.Token == token);
        }
    }

}
