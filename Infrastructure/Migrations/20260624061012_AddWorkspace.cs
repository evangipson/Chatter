using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Conversations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RootPath = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_WorkspaceId",
                table: "Conversations",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Workspace_WorkspaceId",
                table: "Conversations",
                column: "WorkspaceId",
                principalTable: "Workspace",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Workspace_WorkspaceId",
                table: "Conversations");

            migrationBuilder.DropTable(
                name: "Workspace");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_WorkspaceId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Conversations");
        }
    }
}
