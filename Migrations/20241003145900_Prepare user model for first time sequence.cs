using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerApplication.Migrations
{
    /// <inheritdoc />
    public partial class Prepareusermodelforfirsttimesequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyCode",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "newUser",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "newUser",
                table: "AspNetUsers");
        }
    }
}
