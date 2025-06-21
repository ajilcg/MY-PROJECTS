using Microsoft.EntityFrameworkCore;
using BBPSApi.Handlers;
using BBPSApi.Model;
 
namespace BBPSApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserAccounts> UserAccounts { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetUpCommonEntity(modelBuilder);
        }
        protected virtual void SetUpCommonEntity(ModelBuilder modelBuilder)
        {
        }
    }
}

