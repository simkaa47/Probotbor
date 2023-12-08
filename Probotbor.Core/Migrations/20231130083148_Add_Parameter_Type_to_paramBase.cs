using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Probotbor.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddParameterTypetoparamBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParType",
                table: "ParameterBases",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParType",
                table: "ParameterBases");
        }
    }
}
