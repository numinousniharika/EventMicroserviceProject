﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Image")]
    public class ImageController : Controller
    {
        private readonly IHostingEnvironment _env;
        public ImageController(IHostingEnvironment env)
        {
            _env = env;
        }
        [HttpGet]
        [Route("Venue/{id}")]

        public IActionResult GetVenueImage(int id)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot + "/Images/Venues/", "venue" + id + ".png");

            return GetFile(path);
        }

        [HttpGet]
        [Route("Event/{id}")]

        public IActionResult GetEventImage(int id)
        {
            var webRoot = _env.WebRootPath;
            var path = Path.Combine(webRoot + "/Images/Events/", "event" + id + ".png");
            return GetFile(path);
        }

        private IActionResult GetFile(string path)
        {
            try
            {
                var buffer = System.IO.File.ReadAllBytes(path);
                return File(buffer, "image/png");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}