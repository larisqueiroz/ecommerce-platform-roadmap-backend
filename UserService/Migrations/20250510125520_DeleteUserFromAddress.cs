using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class DeleteUserFromAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "PaymentDatas");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PaymentDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "PaymentDatas");

            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "PaymentDatas",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
