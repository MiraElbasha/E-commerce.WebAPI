using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDeliveryMethodShortName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortName",
                table: "DeliveryMethods",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "DeliveryMethods",
                type: "decimal(8,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShortName",
                table: "DeliveryMethods",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "DeliveryMethods",
                type: "decimal(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)");
        }
    }
}
