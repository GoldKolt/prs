using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WebApplication15.Migrations
{
    public partial class DepositAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_DepositContractId",
                table: "Accounts",
                column: "DepositContractId");

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

            migrationBuilder.DropIndex(
                name: "IX_Accounts_DepositContractId",
                table: "Accounts");
        }
    }
}
