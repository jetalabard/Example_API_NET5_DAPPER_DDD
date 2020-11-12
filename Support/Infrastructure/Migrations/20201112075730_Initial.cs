using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Support.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameApp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    VersionApp = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CodeApp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationInfo", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationInfo",
                columns: new[] { "Id", "CodeApp", "NameApp", "VersionApp" },
                values: new object[] { new Guid("85952b6a-1724-4d21-a769-1aa19978291e"), "Example_Code", "Example", "0.1.0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationInfo");
        }
    }
}
