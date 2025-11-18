using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatInRealTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteForeingKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMessages_AppUsers_AppUserId",
                table: "AppMessages");

            migrationBuilder.DropIndex(
                name: "IX_AppMessages_AppUserId",
                table: "AppMessages");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AppMessages");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AppMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AppMessages");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "AppMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppMessages_AppUserId",
                table: "AppMessages",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppMessages_AppUsers_AppUserId",
                table: "AppMessages",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
