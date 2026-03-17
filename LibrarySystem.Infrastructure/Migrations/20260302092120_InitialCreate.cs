using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibrarySystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicationYear = table.Column<int>(type: "int", nullable: false),
                    Pages = table.Column<int>(type: "int", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowerId = table.Column<int>(type: "int", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "BirthYear", "FirstName", "LastName", "Nationality" },
                values: new object[,]
                {
                    { 1, 1965, "J.K.", "Rowling", "UK" },
                    { 2, 1903, "George", "Orwell", "UK" },
                    { 3, 1885, "Karen", "Blixen", "DK" },
                    { 4, 1947, "Stephen", "King", "US" },
                    { 5, 1962, "Suzanne", "Collins", "US" }
                });

            migrationBuilder.InsertData(
                table: "Borrowers",
                columns: new[] { "Id", "Address", "Email", "FirstName", "LastName", "Phone" },
                values: new object[,]
                {
                    { 1, "Testvej 1", "anna@demo.dk", "Anna", "Jensen", "11111111" },
                    { 2, "Testvej 2", "mikkel@demo.dk", "Mikkel", "Hansen", "22222222" },
                    { 3, "Testvej 3", "sara@demo.dk", "Sara", "Nielsen", "33333333" },
                    { 4, "Testvej 4", "jonas@demo.dk", "Jonas", "Pedersen", "44444444" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Genre", "Isbn", "Pages", "PublicationYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Fantasy", "9780747532699", 223, 1997, "Harry Potter 1" },
                    { 2, 1, "Fantasy", "9780747538493", 251, 1998, "Harry Potter 2" },
                    { 3, 2, "Dystopia", "9780451524935", 328, 1949, "1984" },
                    { 4, 2, "Satire", "9780451526342", 112, 1945, "Animal Farm" },
                    { 5, 3, "Memoir", "9780141183332", 416, 1937, "Out of Africa" },
                    { 6, 4, "Horror", "9780307743657", 447, 1977, "The Shining" },
                    { 7, 4, "Horror", "9781501142970", 1138, 1986, "IT" },
                    { 8, 5, "YA", "9780439023528", 374, 2008, "The Hunger Games" },
                    { 9, 5, "YA", "9780439023498", 391, 2009, "Catching Fire" },
                    { 10, 5, "YA", "9780439023511", 390, 2010, "Mockingjay" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookId", "BorrowerId", "DueDate", "LoanDate", "ReturnDate" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 2, 2, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, 3, 3, new DateTime(2026, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, 4, 4, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, 5, 1, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, 6, 2, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, 7, 3, new DateTime(2026, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, 8, 4, new DateTime(2026, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 9, 9, 1, new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, 10, 2, new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 11, 1, 3, new DateTime(2026, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 2, 4, new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 3, 1, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 4, 2, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 5, 3, new DateTime(2026, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 6, 4, new DateTime(2026, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 7, 1, new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 8, 2, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 9, 3, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 10, 4, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookId",
                table: "Loans",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BorrowerId",
                table: "Loans",
                column: "BorrowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Borrowers");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
