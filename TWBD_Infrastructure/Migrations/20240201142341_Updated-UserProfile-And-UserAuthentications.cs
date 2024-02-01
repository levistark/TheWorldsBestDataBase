using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWBD_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserProfileAndUserAuthentications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Authentications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Authentications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
