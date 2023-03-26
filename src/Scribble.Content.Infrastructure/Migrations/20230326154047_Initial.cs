using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scribble.Content.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "blogs",
                columns: table => new
                {
                    blog_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    blog_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    removed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("blogs_pkey", x => x.blog_id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    category_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("categories_pkey", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    tag_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tags_pkey", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    blog_id = table.Column<Guid>(type: "uuid", nullable: false),
                    follower_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("followers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "followers_blog_id_fkey",
                        column: x => x.blog_id,
                        principalTable: "blogs",
                        principalColumn: "blog_id");
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    post_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    title = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    blog_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_posted = table.Column<bool>(type: "boolean", nullable: false),
                    is_rejected = table.Column<bool>(type: "boolean", nullable: false),
                    ViewsCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("posts_pkey", x => x.post_id);
                    table.ForeignKey(
                        name: "posts_blog_id_fkey",
                        column: x => x.blog_id,
                        principalTable: "blogs",
                        principalColumn: "blog_id");
                });

            migrationBuilder.CreateTable(
                name: "blogs_categories",
                columns: table => new
                {
                    blog_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("blogs_categories_pkey", x => new { x.blog_id, x.category_id });
                    table.ForeignKey(
                        name: "blogs_categories_blog_id_fkey",
                        column: x => x.blog_id,
                        principalTable: "blogs",
                        principalColumn: "blog_id");
                    table.ForeignKey(
                        name: "blogs_categories_category_id_fkey",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id");
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    content = table.Column<string>(type: "text", nullable: false),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("comments_pkey", x => x.comment_id);
                    table.ForeignKey(
                        name: "comments_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "post_id");
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    like_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("likes_pkey", x => x.like_id);
                    table.ForeignKey(
                        name: "likes_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "post_id");
                });

            migrationBuilder.CreateTable(
                name: "posts_tags",
                columns: table => new
                {
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tag_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("posts_tags_pkey", x => new { x.post_id, x.tag_id });
                    table.ForeignKey(
                        name: "posts_tags_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "post_id");
                    table.ForeignKey(
                        name: "posts_tags_tag_id_fkey",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "tag_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_blogs_categories_category_id",
                table: "blogs_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "categories_category_name_key",
                table: "categories",
                column: "category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comments_post_id",
                table: "comments",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_followers_blog_id",
                table: "followers",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_post_id",
                table: "likes",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_blog_id",
                table: "posts",
                column: "blog_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_tags_tag_id",
                table: "posts_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "tags_tag_name_key",
                table: "tags",
                column: "tag_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blogs_categories");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "followers");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "posts_tags");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "blogs");
        }
    }
}
