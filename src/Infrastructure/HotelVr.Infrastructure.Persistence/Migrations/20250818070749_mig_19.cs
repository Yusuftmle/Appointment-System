using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRvDbContext.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_passwordResetTokens",
                table: "passwordResetTokens");

            migrationBuilder.RenameTable(
                name: "passwordResetTokens",
                newName: "PasswordResetTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PasswordResetTokens",
                table: "PasswordResetTokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PasswordResetTokens",
                table: "PasswordResetTokens");

            migrationBuilder.RenameTable(
                name: "PasswordResetTokens",
                newName: "passwordResetTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_passwordResetTokens",
                table: "passwordResetTokens",
                column: "Id");
        }
    }
}
