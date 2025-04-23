using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace erp_server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdMaterialTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialTags",
                table: "MaterialTags");

            migrationBuilder.DropIndex(
                name: "IX_MaterialTags_MaterialId",
                table: "MaterialTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MaterialTags");

            migrationBuilder.AlterColumn<Guid>(
                name: "TagId",
                table: "MaterialTags",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialId",
                table: "MaterialTags",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .Annotation("Relational:ColumnOrder", 1)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialTags",
                table: "MaterialTags",
                columns: new[] { "MaterialId", "TagId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MaterialTags",
                table: "MaterialTags");

            migrationBuilder.AlterColumn<Guid>(
                name: "TagId",
                table: "MaterialTags",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<Guid>(
                name: "MaterialId",
                table: "MaterialTags",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "MaterialTags",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaterialTags",
                table: "MaterialTags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTags_MaterialId",
                table: "MaterialTags",
                column: "MaterialId");
        }
    }
}
