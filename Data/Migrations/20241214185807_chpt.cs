using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProiectDAW.Data.Migrations
{
    /// <inheritdoc />
    public partial class chpt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Chapters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_GradeId",
                table: "Chapters",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Grades_GradeId",
                table: "Chapters",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Grades_GradeId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_GradeId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Chapters");
        }
    }
}
