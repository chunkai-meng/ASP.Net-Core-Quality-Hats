using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityHat.Migrations
{
    public partial class orderall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Supplier",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Supplier",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hat",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Hat",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Disc",
                table: "Hat",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customer",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Supplier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Supplier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hat",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Hat",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Disc",
                table: "Hat",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Customer",
                nullable: true);
        }
    }
}
