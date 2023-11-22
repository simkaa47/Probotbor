using Microsoft.EntityFrameworkCore;
using Probotbor.Core.Models.AccessControl;
using Probotbor.Core.Models.Communication;

namespace Probotbor.Core.Infrastructure.DataAccess
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ParameterBase> ParameterBases => Set<ParameterBase>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=vissma_bulk.db");
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public ApplicationContext()
        {

        }


    }
}
