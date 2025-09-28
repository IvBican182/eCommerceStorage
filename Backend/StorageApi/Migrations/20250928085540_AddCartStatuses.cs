using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCartStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CartStatusId",
                table: "Carts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CartStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CartStatusId",
                table: "Carts",
                column: "CartStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_CartStatuses_CartStatusId",
                table: "Carts",
                column: "CartStatusId",
                principalTable: "CartStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_CartStatuses_CartStatusId",
                table: "Carts");

            migrationBuilder.DropTable(
                name: "CartStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CartStatusId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartStatusId",
                table: "Carts");
        }
    }
}
