using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsAPI.Domain
{
    public class Event
    {
        public int EventId { get; set; }

        public string EventName { get; set; }

        public string EventShortDescription { get; set; }

        public string EventLongDescription { get; set; }

        public DateTime EventTime { get; set; }

        public string EventPictureUrl { get; set; }

        public int EventVenueId { get; set; }

        public virtual EventVenue EventVenue { get; set; }

        

        
    }
}
