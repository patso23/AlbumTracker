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
            ArtistRepository repo = new ArtistRepository(_configuration);

            return Ok(repo.Create(new Artist() { Name = artist.Name }));
        }


        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {

            ArtistRepository repo = new ArtistRepository(_configuration);

            return Ok(repo.Get());
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            ArtistRepository repo = new ArtistRepository(_configuration);

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
            ArtistRepository repo = new ArtistRepository(_configuration);

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
        public IActionResult Update(int id, [FromBody] Artist artist)
        {
            ArtistRepository repo = new ArtistRepository(_configuration);

            return Ok(repo.Update(artist, id));
        }

        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            ArtistRepository repo = new ArtistRepository(_configuration);
            repo.DeleteById(id);

            return Ok();
        }


    }
}