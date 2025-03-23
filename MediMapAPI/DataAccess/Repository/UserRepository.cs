using DataAccess.DbContext;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace MediMap.Repositories
{
    public class UserRepository : Repository<ApplicationUser>
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            return await _context.Set<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public virtual async Task<bool> UserExistsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));

            return await _context.Set<ApplicationUser>()
                .AnyAsync(u => u.UserName == username);
        }
    }
}