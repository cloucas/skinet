using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        //Tables to be created in the database
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands{get;set;}
        public DbSet<ProductType> ProductTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //The following is a conversion of decimal types to double; valid for Sqlite database
            //not needed for other databases supporting decimal types
            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType 
                    == typeof(decimal));

                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }   
                }
            }
        }
    }
}