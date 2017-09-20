using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityHat.Data.Migrations
{
    public partial class recipientDefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "Recipient",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "Recipient");
        }
    }
}
