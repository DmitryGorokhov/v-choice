using BLL.DTO;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly IPaginationService _paginationService;
        private readonly ILogger _logger;

        public CommentController(ICrudService cs, IPaginationService ps, ILogger<CommentController> logger)
        {
            _crudService = cs;
            _paginationService = ps;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsPagination([FromQuery] PaginationQuery query)
        {
            _logger.LogInformation("Get pagination comments.");
            if (!ModelState.IsValid || query == null)
            {
                _logger.LogWarning("Get pagination comments: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            var res = await _paginationService.GetCommentsPagination(query);
            if (res == null) return StatusCode(500);

            return Ok(res);
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
            if (commentDTO == null) return StatusCode(500);
            
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
