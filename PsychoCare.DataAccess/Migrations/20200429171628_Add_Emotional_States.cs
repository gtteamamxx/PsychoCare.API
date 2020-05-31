using Microsoft.EntityFrameworkCore.Migrations;

namespace PsychoCare.DataAccess.Migrations
{
    public partial class Add_Emotional_States : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmotionalStates");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmotionalStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnvironmentGroupId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmotionalStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmotionalStates_EnvironmentGroups_EnvironmentGroupId",
                        column: x => x.EnvironmentGroupId,
                        principalTable: "EnvironmentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmotionalStates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmotionalStates_EnvironmentGroupId",
                table: "EmotionalStates",
                column: "EnvironmentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EmotionalStates_UserId",
                table: "EmotionalStates",
                column: "UserId");
        }
    }
}