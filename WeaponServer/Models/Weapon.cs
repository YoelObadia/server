namespace AspNetCoreWebApi6.Models
{
    public class Weapon
    {
        public int Id { get; set; }  // Unique identifier for the weapon
        public string? Name { get; set; }  // Name of the weapon
        public string? Type { get; set; }  // Type of the weapon (e.g., pistol, rifle)
        public string? Manufacturer { get; set; }  // Manufacturer of the weapon
        public string? Caliber { get; set; }  // Caliber of the ammunition used by the weapon
        public int MagazineCapacity { get; set; }  // Magazine Capacity
        public int FireRate { get; set; }  // Fire Rate (rounds per minute)
        public int AmmoCount { get; set; } = 0;  // Current ammo count in the magazine
        public string? Images { get; set; }   // URL of picture of the weapon
    }

}
