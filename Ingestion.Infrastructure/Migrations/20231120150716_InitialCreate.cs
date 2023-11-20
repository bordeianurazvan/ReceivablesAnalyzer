using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ingestion.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditNotes",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IssueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningValue = table.Column<double>(type: "float", nullable: false),
                    PaidValue = table.Column<double>(type: "float", nullable: false),
                    DueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cancelled = table.Column<bool>(type: "bit", nullable: true),
                    DebtorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtorReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtorAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorTown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorZip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorCountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DebtorRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditNotes", x => x.Reference);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Reference = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    IssueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningValue = table.Column<double>(type: "float", nullable: false),
                    PaidValue = table.Column<double>(type: "float", nullable: false),
                    DueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cancelled = table.Column<bool>(type: "bit", nullable: true),
                    DebtorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtorReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebtorAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorTown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorZip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebtorCountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    DebtorRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Reference);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditNotes");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
