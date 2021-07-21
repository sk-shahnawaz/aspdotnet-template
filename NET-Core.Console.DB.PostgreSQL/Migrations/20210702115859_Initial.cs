using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NET.Core.Console.DB.PostgreSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    isbn = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    summary = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    published_on = table.Column<DateTime>(type: "timestamp without time zone", maxLength: 50, nullable: false),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    AuthorId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                    table.ForeignKey(
                        name: "FK_books_authors_AuthorId1",
                        column: x => x.AuthorId1,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "authors",
                columns: new[] { "id", "address", "email", "first_name", "last_name", "middle_name", "phone_number" },
                values: new object[,]
                {
                    { 1L, "CA, US", "adam.freeman@test.com", "Adam", "Freeman", "", "9999999999" },
                    { 2L, "London, UK", "venkat.krishnaswamy@test.com", "Venkat", "Krishnaswamy", "", "9999999998" },
                    { 3L, "Perth, AU", "christopher.williams@test.com", "Christopher", "Williams", "", "9999999997" },
                    { 4L, "WA, US", "sam.johanson@test.com", "Sam", "Johanson", "", "9999999996" },
                    { 5L, "Sao Paolo, Brazil", "mithila.frost@test.com", "Mithila", "Frost", "", "9999999995" },
                    { 6L, "KA, IN", "md.zubair@test.com", "Muhammed", "Zubair", "", "9999999994" },
                    { 7L, "Shanghai, CN", "xi.so.pang@test.com", "Xi", "Pang", "So", "9999999993" },
                    { 8L, "Berlin, DE", "elizabeth.mckinsley@test.com", "Elizabeth", "McKinsley", "", "9999999992" },
                    { 9L, "Del, IN", "ajay.kumar.sharma@test.com", "Ajay", "Sharma", "Kumar", "9999999991" },
                    { 10L, "RJ, IN", "punit.singh@test.com", "Punit", "Singh", "", "9999999990" },
                    { 11L, "Lisbon, Portugal", "geroge.fernandez@test.com", "George", "Fernandez", "", "9999999909" },
                    { 12L, "Tel Aviv, Israel", "ben.andrews.forouzan@test.com", "Ben", "Forouzan", "Andrews", "9999999908" },
                    { 13L, "UP, IN", "ravish.upadhay@test.com", "Ravish", "Upadhay", "", "9999999907" },
                    { 14L, "Montreal, Canada", "amir.nizami@test.com", "Amir", "Nizami", "", "9999999906" },
                    { 15L, "PB, IN", "montel.singh.ahluwalia@test.com", "Montek", "Ahluwalia", "Singh", "9999999905" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_books_AuthorId1",
                table: "books",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_books_isbn",
                table: "books",
                column: "isbn",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "authors");
        }
    }
}
