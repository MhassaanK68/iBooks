using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models
{
    public class UsersDbContexxt : DbContext
    {
        public UsersDbContexxt(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<UsersModel> Users { get; set; }
    }
}
