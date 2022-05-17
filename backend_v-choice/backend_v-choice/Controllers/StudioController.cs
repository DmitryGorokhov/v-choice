using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudioController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly ILogger _logger;

        public StudioController(ICrudService cs, ILogger<StudioController> logger)
        {
            _crudService = cs;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<StudioDTO> GetAll()
        {
            _logger.LogInformation("Get all studios");

            return _crudService.GetAllStudios();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStudio([FromBody] StudioDTO studio)
        {
            _logger.LogInformation("Create studio.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create studio: model state is not valid.");

                return BadRequest(ModelState);
            }

            var studioDTO = await _crudService.CreateStudioAsync(studio);
            if (studioDTO == null) return StatusCode(500);

            return CreatedAtAction("CreateStudio", new { id = studioDTO.Id }, studioDTO);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudio([FromRoute] int id, [FromBody] StudioDTO studio)
        {
            _logger.LogInformation($"Update studio with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update studio: model state is not valid.");

                return BadRequest(ModelState);
            }

            await _crudService.UpdateStudioAsync(id, studio);

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudio([FromRoute] int id)
        {
            _logger.LogInformation($"Delete studio with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete studio: model state is not valid.");

                return BadRequest(ModelState);
            }

            await _crudService.DeleteStudioAsync(id);

            return NoContent();
        }
    }
}
