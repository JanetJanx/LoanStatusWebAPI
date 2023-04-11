using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanStatusWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanPeriod",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanPeriod",
                table: "Loan");
        }
    }
}
