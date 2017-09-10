using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityHat.Data.Migrations
{
    public partial class CustomerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "AspNetUsers");
        }
    }
}
