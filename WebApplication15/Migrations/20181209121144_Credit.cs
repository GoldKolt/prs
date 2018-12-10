using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication15.Migrations
{
    public partial class Credit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsForPercents",
                table: "Accounts",
                newName: "DepositAccount_IsForPercents");

            migrationBuilder.AddColumn<int>(
                name: "CreditContractId",
                table: "Accounts",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForPercents",
                table: "Accounts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HasAnnuityPayments = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    Term = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditContracts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BeginDate = table.Column<DateTime>(nullable: false),
                    CreditAmount = table.Column<decimal>(nullable: false),
                    CreditId = table.Column<int>(nullable: false),
                    Currency = table.Column<int>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditContracts_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditContracts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CreditContractId",
                table: "Accounts",
                column: "CreditContractId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditContracts_CreditId",
                table: "CreditContracts",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditContracts_UserId",
                table: "CreditContracts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_CreditContracts_CreditContractId",
                table: "Accounts",
                column: "CreditContractId",
                principalTable: "CreditContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_CreditContracts_CreditContractId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "CreditContracts");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_CreditContractId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "CreditContractId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsForPercents",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "DepositAccount_IsForPercents",
                table: "Accounts",
                newName: "IsForPercents");
        }
    }
}
