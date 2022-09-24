using Microsoft.EntityFrameworkCore;

namespace CW2_WebClient.Data
{
    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions<LoginDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Login> Customer => Set<Models.Login>();

    }
}
