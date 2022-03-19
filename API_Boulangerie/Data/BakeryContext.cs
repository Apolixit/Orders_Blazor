using Microsoft.EntityFrameworkCore;
using API_Orders;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API_Orders.Data
{
    public class BakeryContext : IdentityDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Portion> Portion { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CategoryProduct> CategoryProduct { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Héritage Produit / Produit Part
            //modelBuilder.Entity<ProduitPart>().HasBaseType<Produit>();
            //https://docs.microsoft.com/fr-fr/ef/core/modeling/relationships -- la bible des relation chips

            modelBuilder.Entity<Product>();
            modelBuilder.Entity<CategoryProduct>();

            modelBuilder.Entity<Client>()
                        .Property(x => x.Disabled)
                        .HasDefaultValue(false);

            modelBuilder.Entity<OrderDetails>()
                .HasOne(cd => cd.order)
                .WithMany(c => c.Details)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public BakeryContext() : base()
        {

        }

        public BakeryContext(DbContextOptions<BakeryContext> options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            ChangeTracker
                .Entries()
                .Where(e => e.Entity is IEntitySaveChange && (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToList()?
                .ForEach(x => {
                    switch(x.State)
                    {
                        case EntityState.Added:
                            ((IEntitySaveChange)x.Entity).onInsert();
                            break;
                        case EntityState.Modified:
                            ((IEntitySaveChange)x.Entity).onUpdate();
                            break;
                    }
                });
            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=Boulangerie;Timeout=30;MultipleActiveResultSets=True;Trusted_Connection=True");
            optionsBuilder.UseSqlServer(@"data source=DESKTOP-HGEHFUL;initial catalog=BarkeryOrders;Timeout=30;MultipleActiveResultSets=True;Trusted_Connection=True");
        }
    }
}
