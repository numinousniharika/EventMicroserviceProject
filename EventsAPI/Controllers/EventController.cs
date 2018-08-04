using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsAPI.Data;
using EventsAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        private readonly EventContext _eventContext;

        //private readonly IOptionSnapshot<EventControllerSettings> _settings;

        public EventController(EventContext eventContext)
        {
            _eventContext = eventContext;
        }

        [HttpGet]
        [Route("[action]")]

        public async Task<IActionResult> EventVenues()
        {
            var items = await _eventContext.EventVenues.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Instances(
            [FromQuery] int pageSize = 2,
            [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _eventContext.Events.LongCountAsync();

            var itemsOnPage = await _eventContext.
                Events.OrderBy(e => e.EventName).
                Skip(pageSize * pageIndex).
                Take(pageSize).
                ToListAsync();
            //itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage)

            return Ok(itemsOnPage);
        }

        [HttpGet]
        [Route("instances/{id:int}")]

        public async Task<IActionResult> GetInstanceById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var item = await _eventContext.Events.SingleOrDefaultAsync(e => e.EventId == id);
                if (item != null)
            {

                //item.pictureUrl = item.PictureUrl.Replace("http://externaleventbaseurltobereplaced","http://localhost:5000 OR _settings.Value.ExternalEventBaseUrl")
                return Ok(item);
            }

            return NotFound();
        }

        private List<Event> ChangeUrlPlaceHolder(List<Event> events)
        {
            events.ForEach(x => x.EventPictureUrl = x.EventPictureUrl.
            Replace("http://externaleventbaseurltobereplaced",
                "http://localhost:5000"));
            return events;
        }


    }
}