using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<UsersModel>? Users { get; set; }
        public DbSet<BooksModel>? Books { get; set; }
        public DbSet<AdminActivityModel>? AdminActivity { get; set; }

    }
}
