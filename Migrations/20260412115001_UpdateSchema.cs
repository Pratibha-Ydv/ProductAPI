using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PrimeResults",
                newName: "ExecutedAt");

            migrationBuilder.AddColumn<int>(
                name: "PrimeCount",
                table: "PrimeResults",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimeCount",
                table: "PrimeResults");

            migrationBuilder.RenameColumn(
                name: "ExecutedAt",
                table: "PrimeResults",
                newName: "CreatedAt");
        }
    }
}
