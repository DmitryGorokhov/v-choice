using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public CommentsController(ICommentsRepository comments)
        {
            _comments = comments;
        }

        [HttpGet("{id}")]
        public IActionResult GetAllCommentsByFilmID([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var comments = _comments.GetAllCommentsAsync(id);
            if(comments == null)
            {
                return NotFound();
            }
            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _comments.CreateCommentAsync(comment, HttpContext.User);
            return CreatedAtAction("GetFilm", new { id = comment.Id }, comment);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _comments.UpdateCommentAsync(id, comment);
                return NoContent();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _comments.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NoContent();
            }
        }
    }
}
