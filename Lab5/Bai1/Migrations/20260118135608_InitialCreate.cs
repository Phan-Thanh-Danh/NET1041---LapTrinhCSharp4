using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bai1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    last_name = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "student_details",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", nullable: false),
                    date_birth = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_details", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_student_details_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_details");

            migrationBuilder.DropTable(
                name: "students");
        }
    }
}
