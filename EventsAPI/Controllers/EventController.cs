using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsAPI.Data;
using EventsAPI.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EventsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        private readonly EventContext _eventContext;

        private readonly IOptionsSnapshot<EventControllerSettings> _settings;

        public EventController(EventContext eventContext, IOptionsSnapshot<EventControllerSettings> settings)
        {
            _eventContext = eventContext;
            _settings = settings;
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
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            return Ok(itemsOnPage);
        }

        [HttpGet]
        [Route("instances/{id:int}", Name = "GetInstanceById")]

        public async Task<IActionResult> GetInstanceById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var item = await _eventContext.Events.SingleOrDefaultAsync(e => e.EventId == id);
            if (item != null)
            {

                item.EventPictureUrl = item.EventPictureUrl.Replace("http://externaleventbaseurltobereplaced",
                    _settings.Value.ExternalEventBaseUrl);
                return Ok(item);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Instances(string name,
            [FromQuery] int pageSize = 2,
            [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _eventContext.Events.
                Where(e => e.EventName.StartsWith(name)).
                LongCountAsync();

            var itemsOnPage = await _eventContext.
                Events.Where(e => e.EventName.StartsWith(name)).OrderBy(e => e.EventName).
                Skip(pageSize * pageIndex).
                Take(pageSize).
                ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);
            //var model = new PaginatedItemsViewModel<Event>(page 


            return Ok(itemsOnPage);
        }

        [HttpGet]
        [Route("[action]/venue/{venueId}")]
        public async Task<IActionResult> Instances(
            int? venueId,
            [FromQuery] int pageSize = 2,
            [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<Event>)_eventContext.Events;

            if (venueId.HasValue)
            {
                root = root.Where(e => e.EventVenueId == venueId);
            }

            var totalItems = await root.LongCountAsync();

            var itemsOnPage = await root.OrderBy(e => e.EventName).
                Skip(pageSize * pageIndex).
                Take(pageSize).
                ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            return Ok(itemsOnPage);
        }

        [HttpGet]
        [Route("instances/month/{month:int}")]
        public async Task<IActionResult> GetInstancesByMonth(
            int? month,
            [FromQuery] int pageSize = 2,
            [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<Event>)_eventContext.Events;

            if (month.HasValue)
            {
                root = root.Where(e => e.EventTime.Year == DateTime.Now.Year 
                && e.EventTime.Month == month);
            }

            var totalItems = await root.LongCountAsync();

            var itemsOnPage = await root.OrderBy(e => e.EventTime).
                Skip(pageSize * pageIndex).
                Take(pageSize).
                ToListAsync();
            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            return Ok(itemsOnPage);
        }

        [HttpPost]
        [Route("instances")]

        public async Task<IActionResult> CreateEvent([FromBody] Event eventToCreate)
        {
            var instance = new Event
            {
                EventId = eventToCreate.EventId,
                EventName = eventToCreate.EventName,
                EventShortDescription = eventToCreate.EventShortDescription,
                EventLongDescription = eventToCreate.EventLongDescription,
                EventTime = eventToCreate.EventTime,
                EventPictureUrl = eventToCreate.EventPictureUrl,
                EventVenueId = eventToCreate.EventVenueId

            };
            if(!instance.EventPictureUrl.StartsWith("http"))
            {
                instance.EventPictureUrl = "http://externaleventbaseurltobereplaced/api/Image/Event/0";
            }
            _eventContext.Events.Add(instance);
            await _eventContext.SaveChangesAsync();
            return CreatedAtAction("GetInstanceById", new { id = instance.EventId });
        }

        [HttpPut]
        [Route("instances")]

        public async Task<IActionResult> UpdateEvent([FromBody] Event eventToUpdate)
        {
            var instance = await _eventContext.Events
                .SingleOrDefaultAsync(e => e.EventId == eventToUpdate.EventId);
            if (instance == null)
            {
                return NotFound(new { Message = $"Event Instance with id {eventToUpdate.EventId} not found." });
            }

            instance = eventToUpdate;
            _eventContext.Events.Update(instance);
            await _eventContext.SaveChangesAsync();

            return CreatedAtRoute("GetInstanceById", new { id = instance.EventId });
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteEvent(int id)
        {
            var instance = await _eventContext.Events.SingleOrDefaultAsync(e => e.EventId == id);
            if(instance == null)
            {
                return NotFound();
            }
            _eventContext.Events.Remove(instance);
            await _eventContext.SaveChangesAsync();
            return NoContent();
        }


        private List<Event> ChangeUrlPlaceHolder(List<Event> events)
        {
            events.ForEach(x => x.EventPictureUrl = x.EventPictureUrl.
            Replace("http://externaleventbaseurltobereplaced",
                _settings.Value.ExternalEventBaseUrl));
            return events;
        }


    }
}