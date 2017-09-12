using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityHat.Migrations
{
    public partial class category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Disc",
                table: "Category",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PaidDate",
                table: "Order",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderedDate",
                table: "Order",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeveliverdDate",
                table: "Order",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Category",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disc",
                table: "Category");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PaidDate",
                table: "Order",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderedDate",
                table: "Order",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeveliverdDate",
                table: "Order",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Category",
                nullable: true);
        }
    }
}
