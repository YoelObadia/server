using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi6.Models
{
    // Database context
    public class WeaponContext: DbContext
    {
        // Constructor
        public WeaponContext(DbContextOptions<WeaponContext> options): base(options)
        {
        }

        // Weapons table
        public DbSet<Weapon> Weapons { get; set; } = null!;
    }
}
