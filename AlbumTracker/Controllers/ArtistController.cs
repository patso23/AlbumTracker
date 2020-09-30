using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AlbumTracker.Controllers
{
    [ApiController]
    [Route("albumtracker.api/[controller]")]
    public class ArtistController : ControllerBase
    {
        private readonly ILogger<ArtistController> _logger;
        private readonly IConfiguration _configuration;

        public ArtistController(ILogger<ArtistController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Artist artist)
        {
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Create(new Artist() { Name = artist.Name }));
        }


        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Get(new List<Artist>()));
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            var result = repo.GetById(new Artist(), id);

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

            var result = repo.GetByName(new Artist(), name);

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
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Update(artist, id));
        }

        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);
            repo.DeleteById(new Artist(), id);

            return Ok();
        }


    }
}