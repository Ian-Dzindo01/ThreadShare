using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreadShare.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ThreadShare.DTOs.Entites;

namespace Controllers.Forums
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;

        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }

        /// <summary>
        /// Displays the page to create a new forum.
        /// </summary>
        [HttpGet("create")]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View(); // This returns the Create view for forums
        }

        /// <summary>
        /// Creates a new forum.
        /// </summary>
        /// <param name="forumViewModel">The forum data to create.</param>
        /// <returns>A success message indicating the forum was created.</returns>
        /// <response code="201">Returns a success message indicating that the forum was created successfully.</response>
        /// <response code="400">If the input data is invalid.</response>
        /// <response code="403">If the user is not authorized.</response>
        [HttpPost("create")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody] ForumViewModel forumViewModel)
        {
            if (forumViewModel == null || string.IsNullOrWhiteSpace(forumViewModel.Name) || string.IsNullOrWhiteSpace(forumViewModel.Description))
            {
                return BadRequest("Both Name and Description are required.");
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            forumViewModel.UserId = userId;

            await _forumService.CreateForum(forumViewModel);

            return Ok("Forum created successfully.");
        }

        /// <summary>
        /// Deletes an existing forum.
        /// </summary>
        /// <param name="forumId">The ID of the forum to delete.</param>
        /// <returns>No content if deletion is successful.</returns>
        /// <response code="204">If the deletion was successful</response>
        /// <response code="404">If the forum was not found</response>
        /// <response code="403">If the user is not authorized</response>
        [HttpDelete("delete/{forumId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int forumId)
        {
            var forum = await _forumService.GetForumById(forumId);
            if (forum == null)
            {
                return NotFound($"Forum with ID {forumId} not found.");
            }

            await _forumService.DeleteForum(forumId);
            return NoContent();
        }
    }
}