using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEP490_G74_RJMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionPlanTarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetPool",
                table: "SubscriptionPlans");

            migrationBuilder.AddColumn<int>(
                name: "TargetAudience",
                table: "SubscriptionPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "SubscriptionPlans");

            migrationBuilder.AddColumn<string>(
                name: "TargetPool",
                table: "SubscriptionPlans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
