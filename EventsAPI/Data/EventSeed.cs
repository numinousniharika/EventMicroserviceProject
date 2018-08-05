using EventsAPI.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Data
{
    public class EventSeed
    {
        public static async Task SeedAsync(EventContext context)
        {
            context.Database.Migrate();
            if (!context.EventVenues.Any())
            {
                context.EventVenues.AddRange(GetPreConfiguredEventVenues());
                await context.SaveChangesAsync();
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(GetPreConfiguredEvents());
                await context.SaveChangesAsync();
            }


        }
        static IEnumerable<EventVenue> GetPreConfiguredEventVenues()
        {
            return new List<EventVenue>()
            {
                new EventVenue() { EventVenueName = "Palais De Mahe"},
                new EventVenue() { EventVenueName = "Tacoma Dome"},
                new EventVenue() { EventVenueName = "The Residence"},
                new EventVenue() { EventVenueName = "Safeco Field"}
            };
        }

        static IEnumerable<Event> GetPreConfiguredEvents()
        {
            return new List<Event>()
            {
                new Event() { EventVenueId = 1, EventName = "Creole Food Festival", EventShortDescription = "A French Affair", EventLongDescription = "Come Enjoy Creole Cuisine", EventTime = new DateTime (2018, 8, 19, 8, 30, 00), EventPictureUrl = "http://externaleventbaseurltobereplaced/api/Image/Event/1"},
                new Event() { EventVenueId = 2, EventName = "Joanne World Tour", EventShortDescription = "Lady Gaga in Concert", EventLongDescription = "Lady Gaga World Tour 2018", EventTime = new DateTime (2018, 8, 25, 22, 30, 00), EventPictureUrl = "http://externaleventbaseurltobereplaced/api/Image/Event/2"},
                new Event() { EventVenueId = 3, EventName = "Mauritian Culture Festival", EventShortDescription = "Mauritian Nights", EventLongDescription = "Enjoy Summer in Paradise", EventTime = new DateTime(2018, 8, 21, 22, 30, 00), EventPictureUrl = "http://externaleventbaseurltobereplaced/api/Image/Event/3"},
                new Event() { EventVenueId = 4, EventName = "Amazon Family Day", EventShortDescription = "Bringing Everyone Joy", EventLongDescription = "Whole Family Enjoys Together", EventTime = new DateTime(2018, 9, 10, 20, 30, 00), EventPictureUrl = "http://externaleventbaseurltobereplaced/api/Image/Event/4"}
            };
        }
    }
}
