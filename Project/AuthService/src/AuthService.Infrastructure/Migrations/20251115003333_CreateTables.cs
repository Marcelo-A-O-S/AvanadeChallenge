using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordSalt = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[,]
                {
                    { 1L, "administrador@teste.com", "Administrador", "+PZDg/QC6WiGmD4PsVox4J93XsQpVgwhf9p7kFoWpZM=", "lxDM6u0i1QzkH9qjLcbddQncvod+GAUnq3P0/SRbDgo2g/A/f5aQxsebHJy0WO5zVWYdHf9iH/NUcd9Ekfo0Xg==", 2 },
                    { 2L, "client@teste.com", "Client", "rib/htc/z3A7WTpkyiq+WS6r9d6UNeKrK9uxsMHNmKc=", "DIWblV8sddpnht/d1qPcIw3giLSrhDT5qbrPfGoML9J7y7EnrMxB96uLqGVLIAmEIw+vutavTmgafLRq4CI9bg==", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
