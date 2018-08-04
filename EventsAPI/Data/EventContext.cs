using EventsAPI.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Data
{
    public class EventContext: DbContext
    {
        
        public EventContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<EventVenue> EventVenues { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating
            (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventVenue>(ConfigureEventVenue);
            modelBuilder.Entity<Event>(ConfigureEvent);
           
            //this table will get built after EventType and EventAttendee since those two need to be built first as EventTicket is dependant on those two.
        }

        private void ConfigureEvent
            (EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");
            builder.Property(c => c.EventId).ForSqlServerUseSequenceHiLo("event_hilo").
                IsRequired();
            
            builder.Property(c => c.EventName).IsRequired().HasMaxLength(100);
            
            builder.Property(c => c.EventTime).IsRequired();
            
            builder.Property(c => c.EventPictureUrl).IsRequired(false);
            
            builder.Property(c => c.EventShortDescription).IsRequired(false);
            
            builder.Property(c => c.EventLongDescription).IsRequired(false);
            builder.HasOne(c => c.EventVenue).
                WithMany().HasForeignKey(c => c.EventVenueId);
        }

        private void ConfigureEventVenue(EntityTypeBuilder<EventVenue> builder)
        {
            builder.ToTable("EventVenue");
            builder.Property(c => c.EventVenueId).ForSqlServerUseSequenceHiLo("eventvenue_hilo").
                IsRequired();
            
            builder.Property(c => c.EventVenueName).IsRequired().HasMaxLength(50);
        }



    
}
}
