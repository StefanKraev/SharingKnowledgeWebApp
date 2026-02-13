using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharingKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialConfigurationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "OpenCourses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FacultyNumber",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OpenCourseStudent",
                columns: table => new
                {
                    EnrolledCoursesId = table.Column<int>(type: "int", nullable: false),
                    EnrolledStudentsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenCourseStudent", x => new { x.EnrolledCoursesId, x.EnrolledStudentsId });
                    table.ForeignKey(
                        name: "FK_OpenCourseStudent_AspNetUsers_EnrolledStudentsId",
                        column: x => x.EnrolledStudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpenCourseStudent_OpenCourses_EnrolledCoursesId",
                        column: x => x.EnrolledCoursesId,
                        principalTable: "OpenCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FacultyNumber", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "007e37ed-27e5-43cb-a8b7-a3b14d054f45", 0, "aa0574e9-1325-431f-a2fa-ed2d7fa83f66", "admin@sharingknowledge.com", true, "0MI0000000", false, null, "ADMIN@SHARINGKNOWLEDGE.COM", "GHOSTADMIN", "AQAAAAIAAYagAAAAEAWlUe2Hax0Inm/pQBMy7iSEi9tdp8v/AnSLrDcnoM+NkHB0gZSu8K9Zf0FzOQNzZw==", null, false, "3235650d-6e47-49f3-9d0a-04664879201a", false, "GhostAdmin" });

            migrationBuilder.UpdateData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatorId",
                value: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.UpdateData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatorId",
                value: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.UpdateData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatorId",
                value: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.UpdateData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatorId",
                value: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.UpdateData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatorId",
                value: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.UpdateData(
                table: "OpenCourses",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatorId",
                value: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.CreateIndex(
                name: "IX_OpenCourses_CreatorId",
                table: "OpenCourses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FacultyNumber",
                table: "AspNetUsers",
                column: "FacultyNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenCourseStudent_EnrolledStudentsId",
                table: "OpenCourseStudent",
                column: "EnrolledStudentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenCourses_AspNetUsers_CreatorId",
                table: "OpenCourses",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenCourses_AspNetUsers_CreatorId",
                table: "OpenCourses");

            migrationBuilder.DropTable(
                name: "OpenCourseStudent");

            migrationBuilder.DropIndex(
                name: "IX_OpenCourses_CreatorId",
                table: "OpenCourses");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FacultyNumber",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "007e37ed-27e5-43cb-a8b7-a3b14d054f45");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "OpenCourses");

            migrationBuilder.DropColumn(
                name: "FacultyNumber",
                table: "AspNetUsers");
        }
    }
}
