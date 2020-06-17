using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComunitateaMea.Data.Migrations
{
    public partial class Files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    FileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    ContentType = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    FileType = table.Column<int>(nullable: false),
                    TicketId = table.Column<int>(nullable: false),
                    TicketId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_Files_Ticket_TicketId1",
                        column: x => x.TicketId1,
                        principalTable: "Ticket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_TicketId1",
                table: "Files",
                column: "TicketId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
