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
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Create(album));

        }

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Get(new List<Album>()));
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            var result = repo.GetById(new Album(), id);

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
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            var result = repo.GetByName(new Album(), name);

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
            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);

            return Ok(repo.Update(album, id));
        }

        //delete
        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult DeleteById(int id)
        {

            Repository.Repository repo = new Repository.Repository(_configuration["connectionString"]);
            repo.DeleteById(new Album(), id);
            return Ok();

        }
    }
}
