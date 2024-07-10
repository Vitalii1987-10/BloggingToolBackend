using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("user/{emailAccountId}/blog/{blogId}/article/{articleId}/")]
public class ArticleCommentController : ControllerBase
{
  private readonly AppDbContext _context;

  public ArticleCommentController(AppDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Adds a comment to an article.
  /// </summary>
  /// <param name="articleId">The ID of the article to add the comment to.</param>
  /// <param name="articleCommentDto">The DTO containing the comment details.</param>
  /// <returns>Returns no content if the comment is successfully added.</returns>
  [HttpPost("add-comment")]
  public async Task<IActionResult> AddComment(int articleId, ArticleCommentDto articleCommentDto) 
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var comment = new ArticleComment 
    {
      ArticleId = articleId,
      CommentatorName = articleCommentDto.CommentatorName,
      Comment = articleCommentDto.Comment,
      CreatedTimestamp = articleCommentDto.CreatedTimestamp
    };

    var response = new ArticleCommentDto 
    {
      CommentatorName = comment.CommentatorName,
      Comment = comment.Comment,
      CreatedTimestamp = comment.CreatedTimestamp
    };

    _context.ArticlesComments.Add(comment);
    await _context.SaveChangesAsync();

    return NoContent();
  }


    /// <summary>
    /// Retrieves all comments for a specified article.
    /// </summary>
    /// <param name="articleId">The ID of the article to retrieve comments for.</param>
    /// <returns>Returns a list of comments associated with the specified article.</returns>
    [HttpGet("get-comments")]
    public async Task<ActionResult<IEnumerable<ArticleCommentDto>>> GetAllComments(int articleId)
    {
        var comments = await _context.ArticlesComments
            .Where(comment => comment.ArticleId == articleId)
            .Select(comment => new ArticleCommentDto
            {
                CommentatorName = comment.CommentatorName,
                Comment = comment.Comment,
                CreatedTimestamp = comment.CreatedTimestamp
            })
            .ToListAsync();

        return Ok(comments);
    }
}