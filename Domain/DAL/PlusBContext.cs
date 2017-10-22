using Domain.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domain.DAL
{
    public class PlusBContext : DbContext
    {
        public PlusBContext() : base("PlusBContext")
        {
        }
        public DbSet<Technology> Technologies {get; set;}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
