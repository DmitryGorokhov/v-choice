using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Interfaces;
using v_choice.Models;

namespace v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : Controller
    {
        private readonly ICommentsRepository _comments;
        private readonly ILogger _logger;

        public CommentsController(ICommentsRepository comments, ILogger<CommentsController> logger)
        {
            _comments = comments;
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
            var comments = _comments.GetAllCommentsAsync(id);
            if(comments == null)
            {
                _logger.LogInformation($"Not Found: comments for film with Id equal {id}.");
                return NotFound();
            }
            _logger.LogInformation($"Ok status: comments for film with Id equal {id} was found.");
            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            _logger.LogInformation("Create comment.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create comment: model state is not valid.");
                return BadRequest(ModelState);
            }
            await _comments.CreateCommentAsync(comment, HttpContext.User);
            _logger.LogInformation($"Create comment: comment with Id equal {comment.Id} was created.");
            return CreatedAtAction("GetFilm", new { id = comment.Id }, comment);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] Comment comment)
        {
            _logger.LogInformation($"Update comment with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update comment: model state is not valid.");
                return BadRequest(ModelState);
            }
            try
            {
                await _comments.UpdateCommentAsync(id, comment);
                _logger.LogInformation($"Update comment: comment with Id equal {id} was updated.");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Update comment with id={id}: {e.Message}.");
                return NoContent();
            }
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
            try
            {
                await _comments.DeleteCommentAsync(id);
                _logger.LogInformation($"Delete comment: comment with Id equal {id} was deleted.");
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"Delete comment with id={id}: {e.Message}.");
                return NoContent();
            }
        }
    }
}
