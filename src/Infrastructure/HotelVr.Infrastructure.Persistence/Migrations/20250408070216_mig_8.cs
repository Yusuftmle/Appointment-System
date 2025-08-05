using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRvDbContext.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostTags");

            migrationBuilder.AddColumn<Guid>(
                name: "BlogPostId",
                table: "BlogTags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BlogTagId",
                schema: "dbo",
                table: "BlogPost",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_BlogPostId",
                table: "BlogTags",
                column: "BlogPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogTags_BlogPost_BlogPostId",
                table: "BlogTags",
                column: "BlogPostId",
                principalSchema: "dbo",
                principalTable: "BlogPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogTags_BlogPost_BlogPostId",
                table: "BlogTags");

            migrationBuilder.DropIndex(
                name: "IX_BlogTags_BlogPostId",
                table: "BlogTags");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "BlogTags");

            migrationBuilder.DropColumn(
                name: "BlogTagId",
                schema: "dbo",
                table: "BlogPost");

            migrationBuilder.CreateTable(
                name: "BlogPostTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlogTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostTags_BlogPost_BlogPostId",
                        column: x => x.BlogPostId,
                        principalSchema: "dbo",
                        principalTable: "BlogPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostTags_BlogTags_BlogTagId",
                        column: x => x.BlogTagId,
                        principalTable: "BlogTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTags_BlogPostId",
                table: "BlogPostTags",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTags_BlogTagId",
                table: "BlogPostTags",
                column: "BlogTagId");
        }
    }
}
