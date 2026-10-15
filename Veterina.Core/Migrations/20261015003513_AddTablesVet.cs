using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterina.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesVet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Vet");

            migrationBuilder.CreateTable(
                name: "Species",
                schema: "Vet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Breed",
                schema: "Vet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpecieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breed_Species_SpecieId",
                        column: x => x.SpecieId,
                        principalSchema: "Vet",
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                schema: "Vet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpecieId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weigth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pet_Species_SpecieId",
                        column: x => x.SpecieId,
                        principalSchema: "Vet",
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pet_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Security",
                        principalTable: "User",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Breed_SpecieId",
                schema: "Vet",
                table: "Breed",
                column: "SpecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_SpecieId",
                schema: "Vet",
                table: "Pet",
                column: "SpecieId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_UserId",
                schema: "Vet",
                table: "Pet",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breed",
                schema: "Vet");

            migrationBuilder.DropTable(
                name: "Pet",
                schema: "Vet");

            migrationBuilder.DropTable(
                name: "Species",
                schema: "Vet");
        }
    }
}
