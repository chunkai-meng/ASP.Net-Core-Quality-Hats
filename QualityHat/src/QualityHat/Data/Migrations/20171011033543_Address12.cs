using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityHat.Data.Migrations
{
    public partial class Address12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Recipient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Recipient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Recipient");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Order");
        }
    }
}
