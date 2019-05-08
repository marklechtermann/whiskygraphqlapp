using Microsoft.EntityFrameworkCore;

namespace WhiskyApp.DataAccess
{
    public class WhiskyDbContext : DbContext
    {
        public WhiskyDbContext(DbContextOptions<WhiskyDbContext> options)
        : base(options)
        {

        }

        public DbSet<Models.WhiskyEntity> Whiskys { get; set; }

        public DbSet<Models.DestilleryEntity> Distilleries { get; set; }
    }
}