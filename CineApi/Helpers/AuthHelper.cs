using CineApi.Data;
using Microsoft.EntityFrameworkCore;

namespace CineApi.Helpers
{
    public static class AuthHelper
    {

        public static async Task<bool> UserExists(string email, AppDbContext context)
        {
            return await context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }
    }
}
