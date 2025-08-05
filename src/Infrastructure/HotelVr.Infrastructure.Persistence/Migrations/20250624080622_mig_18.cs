using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRvDbContext.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Unique index kaldır
            migrationBuilder.DropIndex(
                name: "IX_BlogPost_Slug",
                schema: "dbo",
                table: "BlogPost");

            // 2. Computed column'ı tamamen drop et
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[BlogPost] DROP COLUMN [Slug];");

            // 3. Tekrar computed column olarak ekle
            migrationBuilder.Sql(@"
        ALTER TABLE [dbo].[BlogPost] 
        ADD [Slug] AS LOWER(REPLACE(REPLACE(REPLACE([Title], ' ', '-'), ',', ''), '.', '')) PERSISTED;
    ");

            // 4. Unique index oluştur (filtre yok)
            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_Slug",
                schema: "dbo",
                table: "BlogPost",
                column: "Slug",
                unique: true);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogPost_Slug",
                schema: "dbo",
                table: "BlogPost");

            // Down’da da aynı şekilde slug alanı silinip yeniden eklenmeli
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[BlogPost] DROP COLUMN [Slug];");

            migrationBuilder.Sql(@"
        ALTER TABLE [dbo].[BlogPost] 
        ADD [Slug] AS LOWER(REPLACE(REPLACE(REPLACE([Title], ' ', '-'), ',', ''), '.', '')) PERSISTED
    ");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_Slug",
                schema: "dbo",
                table: "BlogPost",
                column: "Slug",
                unique: true);
        }

    }
}