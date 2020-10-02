using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            SongRepository repo = new SongRepository(_configuration);

            return Ok(repo.Create(song));

        }


        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            SongRepository repo = new SongRepository(_configuration);

            return Ok(repo.Get());
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {

            SongRepository repo = new SongRepository(_configuration);

            var result = repo.GetById(id);

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
            SongRepository repo = new SongRepository(_configuration);

            var result = repo.GetByName(name);

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
            SongRepository repo = new SongRepository(_configuration); 

            return Ok(repo.Update(song, id));
        }

        //delete
        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult DeleteById(int id)
        {

            SongRepository repo = new SongRepository(_configuration);
            repo.DeleteById(id);

            
            return Ok();

        }
    }
}
