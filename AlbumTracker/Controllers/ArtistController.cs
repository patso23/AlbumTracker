using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlbumTracker.Services;
using Microsoft.AspNetCore.Authorization;

namespace AlbumTracker.Controllers
{
    [ApiController]
    [Route("albumtracker.api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;

        public ArtistController(ILogger<ArtistController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Artist artist)
        {
            DataAccess acc = new DataAccess();

            return Ok(acc.Create(new Artist() { Name = artist.Name }));
        }


        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {

            DataAccess acc = new DataAccess();

            return Ok(acc.GetAll(new List<Artist>()));
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            DataAccess acc = new DataAccess();

            var result = acc.GetById(new Artist(), id);

            if (result.Id != 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpGet("name/{name}")]
        [Authorize]
        public IActionResult GetByName(string name)
        {
            DataAccess acc = new DataAccess();

            var result = acc.GetByName(new Artist(), name);

            if (result.Id != 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok(null);
            }
        }

        [HttpPut("id/{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody] Artist artist)
        {
            DataAccess acc = new DataAccess();

            return Ok(acc.Update(id, artist));
        }

        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            DataAccess acc = new DataAccess();
            acc.DeleteById(new Artist(), id);

            return Ok();
        }


    }
}