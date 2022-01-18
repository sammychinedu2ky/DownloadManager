using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DownloadManager.Migrations
{
    public partial class m2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Downloads",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: true),
                    OnlineSize = table.Column<long>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Downloads", x => x.FileId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Downloads");
        }
    }
}
