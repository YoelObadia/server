using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApi6.Models
{
    public class WeaponContext: DbContext
    {
        public WeaponContext(DbContextOptions<WeaponContext> options): base(options)
        {
        }

        public DbSet<Weapon> Weapons { get; set; } = null!;
    }
}
