using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceTrackerApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsertoIcollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_FinanceTrackerApplicationUserId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_FinanceTrackerApplicationUserId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "FinanceTrackerApplicationUserId",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Account",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_UserId",
                table: "Account",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AspNetUsers_UserId",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_UserId",
                table: "Account");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Account",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinanceTrackerApplicationUserId",
                table: "Account",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_FinanceTrackerApplicationUserId",
                table: "Account",
                column: "FinanceTrackerApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AspNetUsers_FinanceTrackerApplicationUserId",
                table: "Account",
                column: "FinanceTrackerApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
