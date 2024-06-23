using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTaskId",
                table: "UserTasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_UserTaskId",
                table: "UserTasks",
                column: "UserTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_UserTasks_UserTaskId",
                table: "UserTasks",
                column: "UserTaskId",
                principalTable: "UserTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_UserTasks_UserTaskId",
                table: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_UserTaskId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "UserTaskId",
                table: "UserTasks");
        }
    }
}
