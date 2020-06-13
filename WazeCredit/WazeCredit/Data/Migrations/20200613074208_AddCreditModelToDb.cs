using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WazeCredit.Data.Migrations
{
    public partial class AddCreditModelToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditApplicationModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ssn = table.Column<string>(nullable: false),
                    Dob = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostalCode = table.Column<int>(nullable: false),
                    Salary = table.Column<double>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CreditApproved = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditApplicationModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditApplicationModel");
        }
    }
}
