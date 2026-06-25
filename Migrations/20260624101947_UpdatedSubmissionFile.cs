using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainineeAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSubmissionFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Checksum",
                table: "SubmissionFiles",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubmissionFiles",
                keyColumn: "Checksum",
                keyValue: null,
                column: "Checksum",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Checksum",
                table: "SubmissionFiles",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
