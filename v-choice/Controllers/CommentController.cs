using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly ILogger _logger;

        public CommentController(ICrudService cs, ILogger<CommentController> logger)
        {
            _crudService = cs;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetAllCommentsByFilmID([FromRoute] int id)
        {
            _logger.LogInformation($"Get all comments for film with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get all comments: model state is not valid.");
                return BadRequest(ModelState);
            }
            var comments = _crudService.GetAllCommentsAsync(id);
            if (comments == null)
            {
                _logger.LogInformation($"Not Found: comments for film with Id equal {id}.");
                return NotFound();
            }
            _logger.LogInformation($"Ok status: comments for film with Id equal {id} was found.");
            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO comment)
        {
            _logger.LogInformation("Create comment.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create comment: model state is not valid.");
                return BadRequest(ModelState);
            }

            var commentDTO = await _crudService.CreateCommentAsync(comment, HttpContext.User);
            if (commentDTO == null)
            {
                return StatusCode(500);
            }

            return CreatedAtAction("CreateComment", new { id = comment.Id }, comment);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] CommentDTO comment)
        {
            _logger.LogInformation($"Update comment with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update comment: model state is not valid.");
                return BadRequest(ModelState);
            }

            await _crudService.UpdateCommentAsync(id, comment);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            _logger.LogInformation($"Delete comment with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete comment: model state is not valid.");
                return BadRequest(ModelState);
            }

            await _crudService.DeleteCommentAsync(id);
            return NoContent();
        }
    }
}
