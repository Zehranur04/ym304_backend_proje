using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrackingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "HabitTrackings");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GoalTrackings");

            migrationBuilder.RenameColumn(
                name: "IsCompletedForToday",
                table: "HabitTrackings",
                newName: "IsCompletedForDay");

            migrationBuilder.AddColumn<decimal>(
                name: "ProgressValue",
                table: "HabitTrackings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "HabitTrackings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ProgressValue",
                table: "GoalTrackings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GoalTrackings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressValue",
                table: "HabitTrackings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HabitTrackings");

            migrationBuilder.DropColumn(
                name: "ProgressValue",
                table: "GoalTrackings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GoalTrackings");

            migrationBuilder.RenameColumn(
                name: "IsCompletedForDay",
                table: "HabitTrackings",
                newName: "IsCompletedForToday");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "HabitTrackings",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "GoalTrackings",
                type: "numeric",
                nullable: true);
        }
    }
}
