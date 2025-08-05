using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRvDbContext.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "BlogPost",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                computedColumnSql: "LOWER(REPLACE(REPLACE(REPLACE(Title, ' ', '-'), ',', ''), '.', ''))",
                stored: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComputedColumnSql: "LOWER(REPLACE(REPLACE(REPLACE(Title, ' ', '-'), ',', ''), '.', ''))",
                oldStored: null);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_Slug",
                schema: "dbo",
                table: "BlogPost",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogPost_Slug",
                schema: "dbo",
                table: "BlogPost");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "dbo",
                table: "BlogPost",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                computedColumnSql: "LOWER(REPLACE(REPLACE(REPLACE(Title, ' ', '-'), ',', ''), '.', ''))",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldComputedColumnSql: "LOWER(REPLACE(REPLACE(REPLACE(Title, ' ', '-'), ',', ''), '.', ''))");
        }
    }
}
