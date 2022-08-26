using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.Server.Entities
{
    public class DataBaseContext : DbContext
    {

        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Document> Document { get; set; }
    }
}
