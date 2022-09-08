using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.Server.Entities
{
    public class DataBaseContext : DbContext
    {

        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Document> Document => Set<Document>();
    }
}
