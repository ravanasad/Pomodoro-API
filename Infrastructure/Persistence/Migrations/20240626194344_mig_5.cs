using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "TaskSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSteps_UserTasks_UserTaskId",
                        column: x => x.UserTaskId,
                        principalTable: "UserTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskSteps_UserTaskId",
                table: "TaskSteps",
                column: "UserTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskSteps");

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
    }
}
