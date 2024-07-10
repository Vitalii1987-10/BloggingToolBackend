using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("user/{emailAccountId}/blog/{blogId}/article/{articleId}/")]
public class ArticleLikeController : ControllerBase
{
  private readonly AppDbContext _context;

  public ArticleLikeController(AppDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Adds a like to an article.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account liking the article.</param>
  /// <param name="articleId">The ID of the article being liked.</param>
  /// <param name="articleLikeDto">The DTO containing the like details.</param>
  /// <returns>Returns the details of the added like.</returns>
  [HttpPost("post-like")]  
  public async Task<IActionResult> AddLike(int emailAccountId, int articleId, ArticleLikeDto articleLikeDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var articleLike = new ArticleLike
    {
      IsLiked = articleLikeDto.IsLiked,
      ArticleId = articleId,
      EmailAccountId = emailAccountId
    };

    _context.ArticleLikes.Add(articleLike);
    await _context.SaveChangesAsync();

    var response = new ArticleLikeGetLikeByIdResponse
    {
      IsLiked = articleLike.IsLiked,
    };

    return CreatedAtAction(nameof(GetLikeById), new {likeId = articleLike.LikeId}, response);
  }


  /// <summary>
  /// Gets a like by email account ID and article ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="articleId">The ID of the article.</param>
  /// <returns>Returns the details of the like.</returns>
  [HttpGet("get-like")]
  public async Task<ActionResult<ArticleLikeGetLikeByIdResponse>> GetLikeById(int emailAccountId, int articleId)
  {
    var articleLike = await _context.ArticleLikes
      .Where(articleLike => articleLike.EmailAccountId == emailAccountId && articleLike.ArticleId == articleId)
      .Select(articleLike => new ArticleLikeGetLikeByIdResponse
      {
        IsLiked = articleLike.IsLiked,
      })
      .FirstOrDefaultAsync();

    if (articleLike == null)
    {
      return NotFound();
    }

    return Ok(articleLike);
  }

  /// <summary>
  /// Gets the total number of likes for an article.
  /// </summary>
  /// <param name="articleId">The ID of the article.</param>
  /// <returns>Returns the total number of likes for the article.</returns>
  [HttpGet("get-likes")]
  public async Task<ActionResult<int>> GetLikesByArticleId(int articleId)
  {
    var totalLikes = await _context.ArticleLikes
      .Where(articleLike => articleLike.ArticleId == articleId)
      .CountAsync();

    return Ok(totalLikes);
  }

  /// <summary>
  /// Deletes a like from an article.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account that liked the article.</param>
  /// <param name="articleId">The ID of the article.</param>
  /// <returns>Returns no content if the like is successfully deleted.</returns>
  [HttpDelete("delete-like")]
  public async Task<IActionResult> DeleteLike(int emailAccountId, int articleId)
  {
    var articleLike = await _context.ArticleLikes
      .Where(articleLike => articleLike.EmailAccountId == emailAccountId && articleLike.ArticleId == articleId)
      .FirstOrDefaultAsync();

    if (articleLike == null)
    {
      return NotFound();
    }

    _context.ArticleLikes.Remove(articleLike);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}