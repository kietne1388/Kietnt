using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFood.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<string>(
            //     name: "MembershipTier",
            //     table: "Users",
            //     type: "nvarchar(max)",
            //     nullable: false,
            //     defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipTier",
                table: "Users");
        }
    }
}
