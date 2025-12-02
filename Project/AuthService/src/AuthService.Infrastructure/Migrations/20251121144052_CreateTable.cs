using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "qHdYgya3+jXN3EvJ4qV+uBYy2IR5wEZkFZnbJ3XqY3c=", "z6BA73RFdQTaMqphM4LL0D9ompdVsRzvON5afnwdWHzZYTUcTZPIlOZSRFX28C7inCt8JnKFVZqknLw3J8RxHA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "wQrdevPOCOalGsrU5gQ6XDpsrkKPWr/Pvx48SIlV0e0=", "P3vB6UENYsruyHs9OZIv47SleIhms3tY2DA9fkCfy129fDTuxvTrYLcBsV3di7uttM9L8J+G905AhlAqB/5ibA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "+PZDg/QC6WiGmD4PsVox4J93XsQpVgwhf9p7kFoWpZM=", "lxDM6u0i1QzkH9qjLcbddQncvod+GAUnq3P0/SRbDgo2g/A/f5aQxsebHJy0WO5zVWYdHf9iH/NUcd9Ekfo0Xg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "rib/htc/z3A7WTpkyiq+WS6r9d6UNeKrK9uxsMHNmKc=", "DIWblV8sddpnht/d1qPcIw3giLSrhDT5qbrPfGoML9J7y7EnrMxB96uLqGVLIAmEIw+vutavTmgafLRq4CI9bg==" });
        }
    }
}
