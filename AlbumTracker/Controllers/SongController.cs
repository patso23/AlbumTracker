using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlbumTracker.Services;

namespace AlbumTracker.Controllers
{
    [ApiController]
    [Route("albumtracker.api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;

        public SongController(ILogger<SongController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Song song)
        {
            DataAccess acc = new DataAccess();

            return Ok(acc.Create(song));

        }

        [HttpPost("songs")]
        [Authorize]
        public IActionResult CreateSongs([FromBody] List<Song> songs)
        {
            DataAccess acc = new DataAccess();

            return Ok(acc.Create(songs));
        }

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            DataAccess acc = new DataAccess();

            return Ok(acc.GetAll(new List<Song>()));
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {

            DataAccess acc = new DataAccess();

            var result = acc.GetById(new Song(), id);

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

            var result = acc.GetByName(new Song(), name);

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
        public IActionResult UpdateById(int id, [FromBody] Song song)
        {
            DataAccess acc = new DataAccess();

            return Ok(acc.Update(id, song));
        }

        //delete
        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult DeleteById(int id)
        {

            DataAccess acc = new DataAccess();
            acc.DeleteById(new Song(), id);
            
            return Ok();

        }
    }
}
