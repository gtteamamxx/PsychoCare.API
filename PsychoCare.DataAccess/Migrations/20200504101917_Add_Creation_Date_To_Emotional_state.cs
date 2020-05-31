using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PsychoCare.DataAccess.Migrations
{
    public partial class Add_Creation_Date_To_Emotional_state : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "EmotionalStates");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM EmotionalStates");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "EmotionalStates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}