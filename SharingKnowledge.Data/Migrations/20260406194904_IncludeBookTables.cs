using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharingKnowledge.Data.Migrations
{
    /// <inheritdoc />
    public partial class IncludeBookTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_CourseCategories_CategoryId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBook_Book_BookId",
                table: "StudentBook");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBook_Students_StudentId",
                table: "StudentBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentBook",
                table: "StudentBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Book",
                table: "Book");

            migrationBuilder.RenameTable(
                name: "StudentBook",
                newName: "StudentBooks");

            migrationBuilder.RenameTable(
                name: "Book",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_StudentBook_BookId",
                table: "StudentBooks",
                newName: "IX_StudentBooks_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_CategoryId",
                table: "Books",
                newName: "IX_Books_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentBooks",
                table: "StudentBooks",
                columns: new[] { "StudentId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_CourseCategories_CategoryId",
                table: "Books",
                column: "CategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBooks_Books_BookId",
                table: "StudentBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBooks_Students_StudentId",
                table: "StudentBooks",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_CourseCategories_CategoryId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBooks_Books_BookId",
                table: "StudentBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBooks_Students_StudentId",
                table: "StudentBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentBooks",
                table: "StudentBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "StudentBooks",
                newName: "StudentBook");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "Book");

            migrationBuilder.RenameIndex(
                name: "IX_StudentBooks_BookId",
                table: "StudentBook",
                newName: "IX_StudentBook_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_CategoryId",
                table: "Book",
                newName: "IX_Book_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentBook",
                table: "StudentBook",
                columns: new[] { "StudentId", "BookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Book",
                table: "Book",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_CourseCategories_CategoryId",
                table: "Book",
                column: "CategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBook_Book_BookId",
                table: "StudentBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBook_Students_StudentId",
                table: "StudentBook",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
