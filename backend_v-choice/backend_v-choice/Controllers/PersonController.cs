using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly IPaginationService _paginationService;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _appEnvironment;

        public PersonController(ICrudService cs, IPaginationService ps, ILogger<PersonController> logger, IWebHostEnvironment ae)
        {
            _crudService = cs;
            _paginationService = ps;
            _logger = logger;
            _appEnvironment = ae;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetPersonsPagination([FromQuery] PaginationQueryBase query)
        {
            _logger.LogInformation("Get pagination persons");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get pagination persons: model state is not valid.");

                return BadRequest(ModelState);
            }

            var res = await _paginationService.GetPersonsPaginationAsync(query);
            if (res == null) return StatusCode(500);

            return Ok(res);
        }

        [Route("all")]
        [HttpGet]
        public IEnumerable<PersonDTO> GetAll()
        {
            _logger.LogInformation("Get all persons");

            return _crudService.GetAllPersons();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PersonDTO person)
        {
            _logger.LogInformation("Create person.");

            var personDTO = await _crudService.CreatePersonAsync(person, _appEnvironment);
            if (personDTO == null) return StatusCode(500);

            return CreatedAtAction("CreatePerson", new { id = personDTO.Id }, personDTO);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] PersonDTO person)
        {
            _logger.LogInformation($"Update person with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update person: model state is not valid.");

                return BadRequest(ModelState);
            }

            string path = await _crudService.UpdatePersonAsync(id, person, _appEnvironment);

            return Ok(new { path = path });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation($"Delete person with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete person: model state is not valid.");

                return BadRequest(ModelState);
            }

            await _crudService.DeletePersonAsync(id);

            return NoContent();
        }
    }
}
