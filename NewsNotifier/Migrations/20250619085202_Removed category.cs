using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsNotifier.Migrations
{
    /// <inheritdoc />
    public partial class Removedcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_Category_CategoryID",
                table: "NewsArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationConfig_Category_CategoryID",
                table: "NotificationConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedArticle_NewsArticles_ArticleID",
                table: "SavedArticle");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedArticle_Users_UserID",
                table: "SavedArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedArticle",
                table: "SavedArticle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "SavedArticle",
                newName: "SavedArticles");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_SavedArticle_UserID",
                table: "SavedArticles",
                newName: "IX_SavedArticles_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_SavedArticle_ArticleID",
                table: "SavedArticles",
                newName: "IX_SavedArticles_ArticleID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "NewsArticles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "NewsArticles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedArticles",
                table: "SavedArticles",
                column: "SavedID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryID");

            migrationBuilder.CreateTable(
                name: "ExternalApis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalApis", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_Categories_CategoryID",
                table: "NewsArticles",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationConfig_Categories_CategoryID",
                table: "NotificationConfig",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedArticles_NewsArticles_ArticleID",
                table: "SavedArticles",
                column: "ArticleID",
                principalTable: "NewsArticles",
                principalColumn: "ArticleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedArticles_Users_UserID",
                table: "SavedArticles",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_Categories_CategoryID",
                table: "NewsArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationConfig_Categories_CategoryID",
                table: "NotificationConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedArticles_NewsArticles_ArticleID",
                table: "SavedArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedArticles_Users_UserID",
                table: "SavedArticles");

            migrationBuilder.DropTable(
                name: "ExternalApis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedArticles",
                table: "SavedArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "NewsArticles");

            migrationBuilder.RenameTable(
                name: "SavedArticles",
                newName: "SavedArticle");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_SavedArticles_UserID",
                table: "SavedArticle",
                newName: "IX_SavedArticle_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_SavedArticles_ArticleID",
                table: "SavedArticle",
                newName: "IX_SavedArticle_ArticleID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "NewsArticles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedArticle",
                table: "SavedArticle",
                column: "SavedID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_Category_CategoryID",
                table: "NewsArticles",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationConfig_Category_CategoryID",
                table: "NotificationConfig",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedArticle_NewsArticles_ArticleID",
                table: "SavedArticle",
                column: "ArticleID",
                principalTable: "NewsArticles",
                principalColumn: "ArticleID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedArticle_Users_UserID",
                table: "SavedArticle",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
