﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Probotbor.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRequiredProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "ParameterBases",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "ParameterBases");
        }
    }
}
