using Microsoft.EntityFrameworkCore;
using API_Boulangerie;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API_Boulangerie.Data
{
    public class BoulangerieContext : IdentityDbContext
    {
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Part> Part { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CategorieProduit> CategorieProduit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Héritage Produit / Produit Part
            //modelBuilder.Entity<ProduitPart>().HasBaseType<Produit>();
            //https://docs.microsoft.com/fr-fr/ef/core/modeling/relationships -- la bible des relation chips

            modelBuilder.Entity<Produit>();
            modelBuilder.Entity<CategorieProduit>();

            modelBuilder.Entity<Client>()
                        .Property(x => x.disabled)
                        .HasDefaultValue(false);

            modelBuilder.Entity<CommandeDetails>()
                .HasOne(cd => cd.commande)
                .WithMany(c => c.details)
                .OnDelete(DeleteBehavior.Cascade);

        }

        public BoulangerieContext() : base()
        {

        }

        public BoulangerieContext(DbContextOptions<BoulangerieContext> options) : base(options)
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
            //optionsBuilder.UseSqlServer(@"data source=SNOKE\SQLEXPRESS;initial catalog=Boulangerie;Timeout=30;MultipleActiveResultSets=True;Trusted_Connection=True");
            optionsBuilder.UseSqlServer(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=Boulangerie;Timeout=30;MultipleActiveResultSets=True;Trusted_Connection=True");
            //PROD
            //optionsBuilder.UseSqlServer(@"Data Source=db807496827.hosting-data.io;Initial Catalog=db807496827;User ID=dbo807496827;Password=#Cortosys-1;Timeout=30;MultipleActiveResultSets=True");
        }
    }
}
