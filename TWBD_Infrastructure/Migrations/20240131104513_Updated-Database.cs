using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWBD_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_PhoneNumbers_PhoneNumberPhoneId",
                table: "Profiles");

            migrationBuilder.DropTable(
                name: "PhoneNumbers");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_PhoneNumberPhoneId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "PhoneNumberPhoneId",
                table: "Profiles",
                newName: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Profiles",
                newName: "PhoneNumberPhoneId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    PhoneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumbers", x => x.PhoneId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PhoneNumberPhoneId",
                table: "Profiles",
                column: "PhoneNumberPhoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_PhoneNumbers_PhoneNumberPhoneId",
                table: "Profiles",
                column: "PhoneNumberPhoneId",
                principalTable: "PhoneNumbers",
                principalColumn: "PhoneId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
