using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Konteh.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEndTimeExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Exams",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Exams");
        }
    }
}
