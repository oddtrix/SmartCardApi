using Microsoft.EntityFrameworkCore;
using SmartCardApi.Models.Cards;

namespace SmartCardApi.Contexts
{
    public class AppDomainDbContext : DbContext
    {
        public AppDomainDbContext(DbContextOptions<AppDomainDbContext> options)
            : base(options) { }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Card>()
                .Property(x => x.LearningRate).HasDefaultValue(0).IsRequired(true)
            .HasField("databaseLearningRate").UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

            modelBuilder.Entity<Card>()
                .Property(x => x.Word).IsRequired(true);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cards)
                .HasForeignKey(c => c.UserId);
        }
    }
}
