using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsNotifier.Migrations
{
    /// <inheritdoc />
    public partial class addedfewmorenewmodelclasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "NewsArticles");

            migrationBuilder.RenameColumn(
                name: "Keywords",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "NewsArticles",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "NewsArticles",
                newName: "ArticleID");

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "NewsArticles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "NewsArticles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notification_NewsArticles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "NewsArticles",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notification_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedArticle",
                columns: table => new
                {
                    SavedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ArticleID = table.Column<int>(type: "int", nullable: false),
                    SavedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedArticle", x => x.SavedID);
                    table.ForeignKey(
                        name: "FK_SavedArticle_NewsArticles_ArticleID",
                        column: x => x.ArticleID,
                        principalTable: "NewsArticles",
                        principalColumn: "ArticleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedArticle_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationConfig",
                columns: table => new
                {
                    ConfigID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationConfig", x => x.ConfigID);
                    table.ForeignKey(
                        name: "FK_NotificationConfig_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationConfig_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_CategoryID",
                table: "NewsArticles",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ArticleID",
                table: "Notification",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationConfig_CategoryID",
                table: "NotificationConfig",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationConfig_UserID",
                table: "NotificationConfig",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SavedArticle_ArticleID",
                table: "SavedArticle",
                column: "ArticleID");

            migrationBuilder.CreateIndex(
                name: "IX_SavedArticle_UserID",
                table: "SavedArticle",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsArticles_Category_CategoryID",
                table: "NewsArticles",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsArticles_Category_CategoryID",
                table: "NewsArticles");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "NotificationConfig");

            migrationBuilder.DropTable(
                name: "SavedArticle");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_NewsArticles_CategoryID",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "NewsArticles");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "NewsArticles");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "Keywords");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "NewsArticles",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "ArticleID",
                table: "NewsArticles",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "NewsArticles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
