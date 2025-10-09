using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreeSpin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrentSpinCountToUserCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentSpinCount",
                table: "UserCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSpinCount",
                table: "UserCampaigns");
        }
    }
}
