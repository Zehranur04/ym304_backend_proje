using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalIsArchived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Goals",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Goals");
        }
    }
}
