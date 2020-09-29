using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlbumTracker.Services;
using Microsoft.AspNetCore.Authorization;

namespace AlbumTracker.API.Controllers
{
    [ApiController]
    [Route("albumtracker.api/[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;

        public AlbumController(ILogger<AlbumController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Album album)
        {
            DataAccess acc = new DataAccess();

            return Ok(acc.Create(album));

        }

        [HttpGet("")]
        [Authorize]
        public IActionResult Get()
        {

            DataAccess acc = new DataAccess();

            return Ok(acc.GetAll(new List<Album>()));
        }

        [HttpGet("id/{id}")]
        [Authorize]
        public IActionResult GetById(int id)
        {

            DataAccess acc = new DataAccess();

            var result = acc.GetById(new Album(), id);

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
            DataAccess acc = new DataAccess();

            var result = acc.GetByName(new Album(), name);

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
            DataAccess acc = new DataAccess();

            return Ok(acc.Update(id, album));
        }

        //delete
        [HttpDelete("id/{id}")]
        [Authorize]
        public IActionResult DeleteById(int id)
        {

            DataAccess acc = new DataAccess();
            acc.DeleteById(new Album(), id);
            return Ok();

        }
    }
}
