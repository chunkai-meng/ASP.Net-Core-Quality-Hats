using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityHat.Migrations
{
    public partial class dateTimeFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkPhone",
                table: "Supplier",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Supplier",
                maxLength: 50,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Supplier",
                nullable: false);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkPhone",
                table: "Supplier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Supplier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Supplier",
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
        }
    }
}
