using Microsoft.EntityFrameworkCore;

namespace SmartCardApi.Models
{
    public class CarDDbContext : DbContext
    {
        public CarDDbContext(DbContextOptions<CarDDbContext> options) 
            : base(options) { }

        public DbSet<Card> Cards { get; set; }
    }
}
