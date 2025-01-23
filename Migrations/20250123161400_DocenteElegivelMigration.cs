using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sgchdAPI.Migrations
{
    /// <inheritdoc />
    public partial class DocenteElegivelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocentesElegiveis",
                columns: table => new
                {
                    DisciplinaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocenteId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocentesElegiveis", x => new { x.DisciplinaId, x.DocenteId });
                    table.ForeignKey(
                        name: "FK_DocentesElegiveis_Disciplinas_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocentesElegiveis_Docentes_DocenteId",
                        column: x => x.DocenteId,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocentesElegiveis_DocenteId",
                table: "DocentesElegiveis",
                column: "DocenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocentesElegiveis");
        }
    }
}
