using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeaponServer.Migrations
{
    public partial class Initial : Migration
    {
        // Create the Weapons table
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Caliber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MagazineCapacity = table.Column<int>(type: "int", nullable: false),
                    FireRate = table.Column<int>(type: "int", nullable: false),
                    AmmoCount = table.Column<int>(type: "int", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                });
        }

        // Drop the Weapons table
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weapons");
        }
    }
}
