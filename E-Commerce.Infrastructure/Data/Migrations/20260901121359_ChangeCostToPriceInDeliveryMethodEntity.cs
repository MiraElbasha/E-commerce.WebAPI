using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCostToPriceInDeliveryMethodEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "DeliveryMethods",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "DeliveryMethods",
                newName: "Cost");
        }
    }
}
