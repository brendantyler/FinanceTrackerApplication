using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerApplication.Migrations
{
    /// <inheritdoc />
    public partial class Add_Asset_bool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAsset",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAsset",
                table: "Account");
        }
    }
}
