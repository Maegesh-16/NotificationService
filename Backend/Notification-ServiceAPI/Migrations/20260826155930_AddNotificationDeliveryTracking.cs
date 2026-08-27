using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification_ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationDeliveryTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "SmsNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "SmsNotifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAttemptOn",
                table: "SmsNotifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "PushNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "PushNotifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAttemptOn",
                table: "PushNotifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "NotificationHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "NotificationHistories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAttemptOn",
                table: "NotificationHistories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "EmailNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "EmailNotifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAttemptOn",
                table: "EmailNotifications",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "SmsNotifications");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "SmsNotifications");

            migrationBuilder.DropColumn(
                name: "LastAttemptOn",
                table: "SmsNotifications");

            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "PushNotifications");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "PushNotifications");

            migrationBuilder.DropColumn(
                name: "LastAttemptOn",
                table: "PushNotifications");

            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "NotificationHistories");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "NotificationHistories");

            migrationBuilder.DropColumn(
                name: "LastAttemptOn",
                table: "NotificationHistories");

            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "EmailNotifications");

            migrationBuilder.DropColumn(
                name: "LastAttemptOn",
                table: "EmailNotifications");
        }
    }
}
