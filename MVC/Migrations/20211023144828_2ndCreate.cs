using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC.Migrations
{
    public partial class _2ndCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountName",
                table: "Accounts",
                column: "AccountName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountName",
                table: "Accounts");
        }
    }
}
