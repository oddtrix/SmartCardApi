using Microsoft.EntityFrameworkCore;

namespace SmartCardApi.Models.Cards
{
    public class CardDbContext : DbContext
    {
        public CardDbContext(DbContextOptions<CardDbContext> options)
            : base(options) { }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
