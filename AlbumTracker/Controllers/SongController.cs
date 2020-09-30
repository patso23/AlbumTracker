using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repository;
using Microsoft.Extensions.Configuration;

namespace AlbumTracker.Controllers
{
    [ApiController]
    [Route("albumtracker.api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ILogger<SongController> _logger;
        private readonly IConfiguration _configuration;

        public SongController(ILogger<SongController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Song song)
        {
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Create(song));

        }

        [HttpPost("songs")]
        [Authorize]
        public IActionResult CreateSongs([FromBody] List<Song> songs)
        {
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Create(songs));
        }

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Get(new List<Song>()));
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            var result = repo.GetById(new Song(), id);

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
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            var result = repo.GetByName(new Song(), name);

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
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]); 

            return Ok(repo.Update(song, id));
        }

        //delete
        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult DeleteById(int id)
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);
            repo.DeleteById(new Song(), id);

            
            return Ok();

        }
    }
}
