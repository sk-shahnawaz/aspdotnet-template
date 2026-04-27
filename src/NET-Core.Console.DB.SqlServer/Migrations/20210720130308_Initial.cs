using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NET.Core.Console.DB.SqlServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PublishedOn = table.Column<DateTime>(type: "datetime2", maxLength: 50, nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    AuthorId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId1",
                        column: x => x.AuthorId1,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Address", "Email", "FirstName", "LastName", "MiddleName", "PhoneNumber" },
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
                name: "IX_Books_AuthorId1",
                table: "Books",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Isbn",
                table: "Books",
                column: "Isbn",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
