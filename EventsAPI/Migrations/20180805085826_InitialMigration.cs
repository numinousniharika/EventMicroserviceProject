using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventsAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "event_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "eventvenue_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "EventVenue",
                columns: table => new
                {
                    EventVenueId = table.Column<int>(nullable: false),
                    EventVenueName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVenue", x => x.EventVenueId);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    EventName = table.Column<string>(maxLength: 100, nullable: false),
                    EventShortDescription = table.Column<string>(nullable: true),
                    EventLongDescription = table.Column<string>(nullable: true),
                    EventTime = table.Column<DateTime>(nullable: false),
                    EventPictureUrl = table.Column<string>(nullable: true),
                    EventVenueId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_EventVenue_EventVenueId",
                        column: x => x.EventVenueId,
                        principalTable: "EventVenue",
                        principalColumn: "EventVenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_EventVenueId",
                table: "Event",
                column: "EventVenueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "EventVenue");

            migrationBuilder.DropSequence(
                name: "event_hilo");

            migrationBuilder.DropSequence(
                name: "eventvenue_hilo");
        }
    }
}
