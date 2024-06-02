using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FA.LibraryManagement.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDb : Migration
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
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(30)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookAuthors",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthors_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookImages_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrowings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BorrowedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Borrowings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BorrowedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fine = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Histories_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoke = table.Column<bool>(type: "bit", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateExpire = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowingId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    DueTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Fine = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowingDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowingDetails_Borrowings_BorrowingId",
                        column: x => x.BorrowingId,
                        principalTable: "Borrowings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Stephen Breyer" },
                    { 2, "Dr. Seuss" },
                    { 3, "Robert Greene" },
                    { 4, "Crystal Radke" },
                    { 5, "The Beginner's Bible" },
                    { 6, "Adam Higginbotham" },
                    { 7, "Publications International Ltd" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Law" },
                    { 2, "Teen & Young Adult" },
                    { 3, "Politics & Social Sciences" },
                    { 4, "Education & Teaching" },
                    { 5, "Engineering & Transportation" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName", "RoleId" },
                values: new object[,]
                {
                    { 1, null, "This is role : librarian", "Librarian", "LIBRARIAN", 0 },
                    { 2, null, "This is role : member", "Member", "MEMBER", 0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "Gender", "ImageUrl", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(1999, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "trungsangle123@gmail.com", false, "Trung", "Male", null, "Sang", false, null, "TRUNGSANGLE123@GMAIL.COM", "SANGLT12", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "SangLT12" },
                    { 2, 0, new DateTime(2001, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "hao.nn151201@gmail.com", false, "Ngoc", "Male", null, "Hao", false, null, "HAONN1@GMAIL.COM", "HAONN1", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "HaoNN1" },
                    { 3, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "vomanhdung276@gmail.com", false, "Vo", "Male", null, "Dung", false, null, "DUNGVM7@GMAIL.COM", "DUNGVM7", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "DungVM7" },
                    { 4, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member001@gmail.com", false, "Member", "Male", null, "Test 001", false, null, "MEMBER001@GMAIL.COM", "MEMBER001", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member001" },
                    { 5, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member002@gmail.com", false, "Member", "Male", null, "Test 002", false, null, "MEMBER002@GMAIL.COM", "MEMBER002", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member002" },
                    { 6, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member003@gmail.com", false, "Member", "Male", null, "Test 003", false, null, "MEMBER003@GMAIL.COM", "MEMBER003", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member003" },
                    { 7, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member004@gmail.com", false, "Member", "Male", null, "Test 004", false, null, "MEMBER004@GMAIL.COM", "MEMBER004", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member004" },
                    { 8, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member005@gmail.com", false, "Member", "Male", null, "Test 005", false, null, "MEMBER005@GMAIL.COM", "MEMBER005", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member005" },
                    { 9, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member006@gmail.com", false, "Member", "Male", null, "Test 006", false, null, "MEMBER006@GMAIL.COM", "MEMBER006", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member006" },
                    { 10, 0, new DateTime(2001, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "225095a0-5ce9-4ebc-a51c-08d11a168f78", "member007@gmail.com", false, "Member", "Male", null, "Test", false, null, "MEMBER007@GMAIL.COM", "MEMBER007", "AQAAAAIAAYagAAAAEF+kflztPRzXy/uVDeXi7oc4yNo5ze+AtYcauF4WpN9sZX4/bUJZXgWBftHDzeAVRw==", "0123456789", false, null, "23XYSO4Z3J4WWLXHGVQ35GIRRDPNVN2R", false, "Member007" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "CategoryId", "Description", "ISBN", "PublishedDate", "Publisher", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "A provocative, brilliant analysis by recently retired Supreme Court Justice Stephen Breyer that deconstructs the textualist philosophy of the current Supreme Court’s supermajority and makes the case for a better way to interpret the Constitution.\n\n“You will not read a more important legal work this election year.” —Bob Woodward,Washington Post reporter and author of fifteen #1 New York Times bestselling books\n“A dissent for the ages.” —The Washington Post\n“Breyer’s candor about the state of the court is refreshing and much needed.” —The Boston Globe\n\nThe relatively new judicial philosophy of textualism dominates the Supreme Court. Textualists claim that the right way to interpret the Constitution and statutes is to read the text carefully and examine the language as it was understood at the time the documents were written.\n\nThis, however, is not Justice Breyer’s philosophy nor has it been the traditional way to interpret the Constitution since the time of Chief Justice John Marshall. Justice Breyer recalls Marshall’s exhortation that the Constitution must be a workable set of principles to be interpreted by subsequent generations.\n\nMost important in interpreting law, says Breyer, is to understand the purposes of statutes as well as the consequences of deciding a case one way or another. He illustrates these principles by examining some of the most important cases in the nation’s history, among them the Dobbs and Bruen decisions from 2022 that he argues were wrongly decided and have led to harmful results.\n", "978-1668021538", new DateTime(2024, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Simon & Schuster", 10, "Reading the Constitution: Why I Chose Pragmatism, Not Textualism" },
                    { 2, 1, "A sitting justice reflects upon the authority of the Supreme Court―how that authority was gained and how measures to restructure the Court could undermine both the Court and the constitutional system of checks and balances that depends on it.\n\nA growing chorus of officials and commentators argues that the Supreme Court has become too political. On this view the confirmation process is just an exercise in partisan agenda-setting, and the jurists are no more than “politicians in robes”―their ostensibly neutral judicial philosophies mere camouflage for conservative or liberal convictions.", "978-0674269361", new DateTime(2021, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harvard University Press", 20, "The Authority of the Court and the Peril of Politics" },
                    { 3, 2, "Say “happy birthday,” Dr. Seuss-style! This classic picture book whisks readers away on the most spectacular birthday of all time—and reminds them to celebrate themselves every day of the year!\n\nI wish we could do what they do in Katroo.\nThey sure know how to say “Happy birthday to you!”\n\nWhen the Great Birthday Bird of Katroo arrives to usher in your “Day of all Days,” you can expect a colorful romp full of fantastical fun that is all about YOU! Treat yourself to flowers that smell like licorice and cheese. Pick out the world’s tallest pet—or a nice Time-Telling Fish. Then prepare for a party so grand it will take twenty days just to sweep up the mess.\n\nFeaturing birthday festivities on every page, this joyful classic from the one and only Dr. Seuss rejoices in the person you were born to be!", "978-0394800769", new DateTime(1959, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Random House Books for Young Readers", 20, "Happy Birthday to You!" },
                    { 4, 2, "Say “happy birthday,” Dr. Seuss-style! This classic picture book whisks readers away on the most spectacular birthday of all time—and reminds them to celebrate themselves every day of the year!\n\nI wish we could do what they do in Katroo.\nThey sure know how to say “Happy birthday to you!”\n\nWhen the Great Birthday Bird of Katroo arrives to usher in your “Day of all Days,” you can expect a colorful romp full of fantastical fun that is all about YOU! Treat yourself to flowers that smell like licorice and cheese. Pick out the world’s tallest pet—or a nice Time-Telling Fish. Then prepare for a party so grand it will take twenty days just to sweep up the mess.\n\nFeaturing birthday festivities on every page, this joyful classic from the one and only Dr. Seuss rejoices in the person you were born to be!", "978-0375872334", new DateTime(2013, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Random House Books for Young Readers", 20, "Oh Say Can You Say? (Dr. Seuss Collector's Edition)" },
                    { 5, 3, "Amoral, cunning, ruthless, and instructive, this multi-million-copy New York Times bestseller is the definitive manual for anyone interested in gaining, observing, or defending against ultimate control – from the author of The Laws of Human Nature.\n\nIn the book that People magazine proclaimed “beguiling” and “fascinating,” Robert Greene and Joost Elffers have distilled three thousand years of the history of power into 48 essential laws by drawing from the philosophies of Machiavelli, Sun Tzu, and Carl Von Clausewitz and also from the lives of figures ranging from Henry Kissinger to P.T. Barnum.\n \nSome laws teach the need for prudence (“Law 1: Never Outshine the Master”), others teach the value of confidence (“Law 28: Enter Action with Boldness”), and many recommend absolute self-preservation (“Law 15: Crush Your Enemy Totally”). Every law, though, has one thing in common: an interest in total domination. In a bold and arresting two-color package, The 48 Laws of Power is ideal whether your aim is conquest, self-defense, or simply to understand the rules of the game.", "978-0140280197", new DateTime(2000, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Penguin Books", 20, "The 48 Laws of Power" },
                    { 6, 3, "Please Note That The Following Individual Books As Per Original ISBN and Cover Image In this Listing shall be Dispatched The Daily Laws and The 33 Strategies of War by Robert Greene 2 Books Collection Set : The Daily Over the last 25 years, Robert Greene has provided insights into every aspect of being whether that be getting what you want, understanding others' motivations, mastering your impulses, or recognising strengths and weaknesses. The Daily Laws distills that wisdom into easy-to-digest daily entries whose content spans power, seduction, war, strategy, politics, productivity, psychology, leadership, and adversity.Not only is this beautifully designed volume the perfect entry point for those new to Greene's penetrating insight. The 33 Strategies of From bestselling author Robert Greene comes a brilliant distillation of the strategies of war that can help us gain mastery in the modern world. Spanning world civilisations, and synthesising dozens of political, philosophical, and religious texts, The 33 Strategies of War is a comprehensive guide to the subtle social game of everyday life. Based on profound, timeless lessons, it is abundantly illustrated with examples of the genius and folly of everyone from Napoleon to Margaret Thatcher and Hannibal to Ulysses S. Grant, as well as diplomats, captains of industry and Samurai swordsmen.", "978-9124292041", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Profile Books", 20, "The Daily Laws and The 33 Strategies of War by Robert Greene 2 Books Collection Set" },
                    { 7, 4, "Help your little one build communication skills with the ultimate writing workbook for kids ages 3 to 5. More than 1 million copies sold!\n\nSet kids up to succeed in school with a learn to write for kids guide that offers letter, shape, and number practice for kindergarten—and beyond. My First Learn-to-Write Workbook introduces early writers to proper pen control, line tracing, and more with dozens of handwriting exercises that engage their minds and boost their reading and writing comprehension.", "978-1641526272", new DateTime(2019, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rockridge Press", 20, "My First Learn-to-Write Workbook: Practice for Kids with Pen Control, Line Tracing, Letters, and More!" },
                    { 8, 4, "Millions of children and their parents can’t be wrong. The bright and vibrant illustrations enhance every word of The Beginner’s Bible® to produce one of the most moving and memorable Bible experiences a young child can have.\n\nThe Beginner’s Bible is where a child’s journey towards a lifelong love of God’s Word begins.\n\nKids will enjoy reading the story of Noah’s Ark as they see Noah helping the elephant onto the big boat. They will learn about the prophet Jonah as they see him praying inside the fish. And they will follow along with the text of Jesus’ ministry as they see a man in need of healing lowered down through the roof of a house.\n\nParents, teachers, pastors, and children will rediscover these beloved parables and so much more as they read more than 90 stories in The Beginner's Bible, just like millions of children before. The Beginner’s Bible® brand has been trusted for nearly 30 years, with more than 25 million products sold.", "978-0310750130", new DateTime(2016, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zonderkidz", 20, "The Beginner's Bible: Timeless Children's Stories" },
                    { 9, 5, "The legendary New York Times bestselling tale of top-down change for anyone trying to navigate today's uncertain business seas.\n\nWhen Captain Abrashoff took over as commander of USS Benfold, it was like a business that had all the latest technology but only some of the productivity. Knowing that responsibility for improving performance rested with him, he realized he had to improve his own leadership skills before he could improve his ship. Within months, he created a crew of confident and inspired problem-solvers eager to take the initiative and responsibility for their actions. The slogan on board became \"It's your ship,\" and Benfold was soon recognized far and wide as a model of naval efficiency. How did Abrashoff do it? Against the backdrop of today's United States Navy, Abrashoff shares his secrets of successful management including:\n", "978-1455523023", new DateTime(2012, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grand Central Publishing", 20, "It's Your Ship: Management Techniques from the Best Damn Ship in the Navy, 10th Anniversary Edition" },
                    { 10, 5, "100 Cars That Changed the World showcases vehicles from the end of the nineteenth century to 2020.\nCars are showcased with brief text, color photography, and vintage black and white photography.\nVehicles included:\nThe Ford Model T that put America on wheels.\nThe Volkswagen Beetle that was loved around the world.\nThe Jeep that helped win World War II and popularized off-road adventure.\nThe Pontiac GTO that launched the muscle car era.\nThe Dodge Caravan that changed the way that families travel.\nThe Ford Explorer that ignited the SUV movement.\nThe Tesla Model S that made electric cars exciting.\nAnd many more!\nLarge hardcover book, 144 pages.", "978-1645581246", new DateTime(2020, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Walter Foster Publishing", 20, "100 Cars That Changed the World: The Designs, Engines, and Technologies That Drive Our Imaginations" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "BookAuthors",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 5 },
                    { 3, 6 },
                    { 4, 7 },
                    { 5, 8 },
                    { 6, 9 },
                    { 7, 10 }
                });

            migrationBuilder.InsertData(
                table: "BookImages",
                columns: new[] { "Id", "BookId", "ImageUrl" },
                values: new object[,]
                {
                    { 1, 1, "https://m.media-amazon.com/images/I/6118UMWll-L._SL1500_.jpg" },
                    { 2, 1, "https://m.media-amazon.com/images/I/71agAaqCyOL._SL1500_.jpg" },
                    { 3, 2, "https://m.media-amazon.com/images/I/51B5-WqrHVS._SL1000_.jpg" },
                    { 4, 3, "https://m.media-amazon.com/images/I/61-66t0NIpL._SL1200_.jpg" },
                    { 5, 4, "https://m.media-amazon.com/images/I/91i1FNtjk4L._SL1500_.jpg" },
                    { 6, 5, "https://m.media-amazon.com/images/I/611X8GI7hpL._SL1500_.jpg" },
                    { 7, 6, "https://m.media-amazon.com/images/I/81CE5Iz9VwL._SL1500_.jpg" },
                    { 8, 6, "https://m.media-amazon.com/images/I/91-99A2X5dL._SL1500_.jpg" },
                    { 9, 6, "https://m.media-amazon.com/images/I/71SOwtHPbBL._SL1500_.jpg" },
                    { 10, 7, "https://m.media-amazon.com/images/I/71ZIPHcr-PL._SL1500_.jpg" },
                    { 11, 7, "https://m.media-amazon.com/images/I/71P9+nFqnhL._SL1500_.jpg" },
                    { 12, 7, "https://m.media-amazon.com/images/I/71dxLbzeEoL._SL1500_.jpg" },
                    { 13, 7, "https://m.media-amazon.com/images/I/71EYOUrNnXL._SL1500_.jpg" },
                    { 14, 7, "https://m.media-amazon.com/images/I/71ZIPHcr-PL._SL1500_.jpg" },
                    { 15, 7, "https://m.media-amazon.com/images/I/71qx0ZSysML._SL1500_.jpg" },
                    { 16, 7, "https://m.media-amazon.com/images/I/71d6Ex5i4zL._SL1500_.jpg" },
                    { 17, 7, "https://m.media-amazon.com/images/I/71D8-vq6AYL._SL1500_.jpg" },
                    { 18, 8, "https://m.media-amazon.com/images/I/81Pic6bOvuL._SL1500_.jpg" },
                    { 19, 8, "https://m.media-amazon.com/images/I/91OZJW0dZnL._SL1500_.jpg" },
                    { 20, 9, "https://m.media-amazon.com/images/I/71wWNi5YymL._SL1500_.jpg" },
                    { 21, 9, "https://m.media-amazon.com/images/I/81zNT-wG8hL._SL1500_.jpg" },
                    { 22, 10, "https://m.media-amazon.com/images/I/71dNRskgj7L._SL1500_.jpg" },
                    { 23, 10, "https://m.media-amazon.com/images/I/81LDFz47KSL._SL1500_.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookImages_BookId",
                table: "BookImages",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingDetails_BookId",
                table: "BorrowingDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowingDetails_BorrowingId",
                table: "BorrowingDetails",
                column: "BorrowingId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_UserId",
                table: "Borrowings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_BookId",
                table: "Carts",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_BookId",
                table: "Histories",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_UserId",
                table: "Histories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthors");

            migrationBuilder.DropTable(
                name: "BookImages");

            migrationBuilder.DropTable(
                name: "BorrowingDetails");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Histories");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Borrowings");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
