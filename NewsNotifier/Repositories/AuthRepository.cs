using NewsNotifier.Data;
using NewsNotifier.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Server.Repositories.Interfaces;

namespace NewsAggregator.Server.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .SingleOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
