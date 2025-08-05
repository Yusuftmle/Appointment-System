using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelRvDbContext.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "dbo",
                table: "BlogPost",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_UserId",
                schema: "dbo",
                table: "BlogPost",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_user_UserId",
                schema: "dbo",
                table: "BlogPost",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_user_UserId",
                schema: "dbo",
                table: "BlogPost");

            migrationBuilder.DropIndex(
                name: "IX_BlogPost_UserId",
                schema: "dbo",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "BlogPost");
        }
    }
}
