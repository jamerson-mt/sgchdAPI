using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgchdAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdjusteDocenteEmailUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Docentes_Email",
                table: "Docentes",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Docentes_Email",
                table: "Docentes");
        }
    }
}
