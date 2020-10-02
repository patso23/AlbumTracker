using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace AlbumTracker.API.Controllers
{
    [ApiController]
    [Route("albumtracker.api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IConfiguration _configuration;

        public AlbumController(ILogger<AlbumController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Album album)
        {
            AlbumRepository repo = new AlbumRepository(_configuration);

            return Ok(repo.Create(album));

        }

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            AlbumRepository repo = new AlbumRepository(_configuration);

            return Ok(repo.Get());
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {

            AlbumRepository repo = new AlbumRepository(_configuration);

            var result = repo.GetById(id);

            if(result.Id != 0)
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
            AlbumRepository repo = new AlbumRepository(_configuration);

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
        public IActionResult UpdateById(int id, [FromBody] Album album)
        {
           AlbumRepository repo = new AlbumRepository(_configuration);

            return Ok(repo.Update(album, id));
        }

        //delete
        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult DeleteById(int id)
        {

            AlbumRepository repo = new AlbumRepository(_configuration);
            repo.DeleteById(id);
            return Ok();

        }
    }
}
