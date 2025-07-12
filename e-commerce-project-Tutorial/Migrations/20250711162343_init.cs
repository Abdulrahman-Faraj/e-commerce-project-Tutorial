using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace e_commerce_project_Tutorial.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MainImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Brand", "Description", "Discount", "MainImageData", "MainImageFileName", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "AlphaTech", "A smart and stylish watch from AlphaTech.", 5m, null, null, "Alpha Watch", 199.99m },
                    { 2, "AlphaTech", "A high-performance mobile device by AlphaTech.", 10m, null, null, "Alpha Mobile", 499.99m },
                    { 3, "AlphaTech", "A lightweight and powerful laptop from AlphaTech.", 15m, null, null, "Alpha Laptop", 999.99m },
                    { 4, "BetaWorks", "An elegant watch featuring modern functionalities by BetaWorks.", 5m, null, null, "Beta Watch", 149.99m },
                    { 5, "BetaWorks", "An advanced mobile device with cutting-edge technology from BetaWorks.", 10m, null, null, "Beta Mobile", 599.99m },
                    { 6, "BetaWorks", "A powerful laptop built for both gaming and professional use by BetaWorks.", 20m, null, null, "Beta Laptop", 1099.99m },
                    { 7, "GammaCorp", "A sporty watch with fitness tracking features from GammaCorp.", 5m, null, null, "Gamma Watch", 129.99m },
                    { 8, "GammaCorp", "A compact mobile device with excellent battery life by GammaCorp.", 5m, null, null, "Gamma Mobile", 399.99m },
                    { 9, "GammaCorp", "A versatile laptop with long-lasting battery performance from GammaCorp.", 10m, null, null, "Gamma Laptop", 899.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
