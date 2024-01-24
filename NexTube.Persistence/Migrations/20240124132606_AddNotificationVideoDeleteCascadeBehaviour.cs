﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexTube.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationVideoDeleteCascadeBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Videos_NotificationDataId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Videos_NotificationDataId",
                table: "Notifications",
                column: "NotificationDataId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Videos_NotificationDataId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Videos_NotificationDataId",
                table: "Notifications",
                column: "NotificationDataId",
                principalTable: "Videos",
                principalColumn: "Id");
        }
    }
}
