using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolNewspaperBlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoverFileId",
                table: "Articles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverFileId",
                table: "Articles");
        }
    }
}
