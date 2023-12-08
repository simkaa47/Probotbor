using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Probotbor.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParameterBases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ModbusRegType = table.Column<int>(type: "INTEGER", nullable: false),
                    RegNum = table.Column<int>(type: "INTEGER", nullable: false),
                    BitNum = table.Column<int>(type: "INTEGER", nullable: false),
                    DbNum = table.Column<int>(type: "INTEGER", nullable: false),
                    ByteNum = table.Column<int>(type: "INTEGER", nullable: false),
                    Length = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterBases", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParameterBases");
        }
    }
}
