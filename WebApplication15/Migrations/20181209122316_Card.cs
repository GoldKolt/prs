using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication15.Migrations
{
    public partial class Card : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_DepositContracts_DepositContractId",
                table: "Accounts");

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<int>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Pin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_DepositContracts_DepositContractId",
                table: "Accounts",
                column: "DepositContractId",
                principalTable: "DepositContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_DepositContracts_DepositContractId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_DepositContracts_DepositContractId",
                table: "Accounts",
                column: "DepositContractId",
                principalTable: "DepositContracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
