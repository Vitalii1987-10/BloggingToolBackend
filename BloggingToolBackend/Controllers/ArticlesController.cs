using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("user/{emailAccountId}/blog/{blogId}/")]
public class ArticlesController : ControllerBase 
{
  private readonly AppDbContext _context;

  public ArticlesController(AppDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Adds a new article to a specific blog.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="blogId">The ID of the blog to which the article will be added.</param>
  /// <param name="articleDto">The article DTO containing article details.</param>
  [HttpPost("add-article")]
  public async Task<IActionResult> AddArticle(int emailAccountId, int blogId, ArticleDto articleDto)
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var article = new Article
    {
      ArticleTitle = articleDto.ArticleTitle,
      ArticleAuthor = articleDto.ArticleAuthor,
      ArticleStatus = articleDto.ArticleStatus,
      CreatedTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"),
      UpdatedTimestamp = "-",
      PublishedTimestamp = "-",
      ArticleViewsCount = articleDto.ArticleViewsCount,
      Content = articleDto.Content,
      BlogId = blogId
    };

    var response = new ArticleOnCreateResponseDto {
      ArticleTitle = article.ArticleTitle,
      ArticleAuthor = article.ArticleAuthor,
      ArticleStatus = article.ArticleStatus,
      CreatedTimestamp = article.CreatedTimestamp,
      ArticleViewsCount = article.ArticleViewsCount,
      Content = article.Content,
      BlogId = article.BlogId
    };

    _context.Articles.Add(article);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetArticleById), new { emailAccountId, blogId, articleId = article.ArticleId}, response);
  }

    /// <summary>
    /// Increments the view count for an article.
    /// </summary>
    /// <param name="articleId">The ID of the article.</param>
    /// <returns>Returns no content if the update is successful.</returns>
    [HttpPost("article/{articleId}/increment-views")]
    public async Task<IActionResult> IncrementViews(int articleId)
    {
        var article = await _context.Articles
            .FirstOrDefaultAsync(article => article.ArticleId == articleId);

        if (article == null)
        {
            return NotFound();
        }

        article.ArticleViewsCount++;
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();

        return NoContent();
    }

  /// <summary>
  /// Retrieves an article by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="blogId">The ID of the blog.</param>
  /// <param name="articleId">The ID of the article to retrieve.</param>
  [HttpGet("article/{articleId}")]
  public async Task<ActionResult<GetArticleByIdResponseDto>> GetArticleById(int emailAccountId, int blogId, int articleId)
  {
    var article = await _context.Articles
        .Where(article => article.ArticleId == articleId && article.BlogId == blogId)
        .Select(article => new GetArticleByIdResponseDto {
          ArticleTitle = article.ArticleTitle,
          ArticleAuthor = article.ArticleAuthor,
          Content = article.Content
        })
        .FirstOrDefaultAsync();

    if(article == null)
    {
      return NotFound();
    }

    return Ok(article);
  }

  /// <summary>
  /// Retrieves an article by its ID, including view count.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="blogId">The ID of the blog.</param>
  /// <param name="articleId">The ID of the article to retrieve.</param>
  [HttpGet("reader-article/{articleId}")]
  public async Task<ActionResult<GetArticleByIdResponseDto>> ReaderGetArticleById(int emailAccountId, int blogId, int articleId)
  {
    var article = await _context.Articles
        .Where(article => article.ArticleId == articleId && article.BlogId == blogId)
        .Select(article => new ReaderGetArticleByIdResponseDto
        {
            ArticleTitle = article.ArticleTitle,
            ArticleAuthor = article.ArticleAuthor,
            Content = article.Content,
            ArticleViewsCount = article.ArticleViewsCount // Include the views count
        })
        .FirstOrDefaultAsync();

    if (article == null)
    {
        return NotFound();
    }

    return Ok(article);
}

  /// <summary>
  /// Retrieves all articles associated with a specific blog.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="blogId">The ID of the blog whose articles to retrieve.</param>
  [HttpGet("articles")]
  public async Task<ActionResult<IEnumerable<GetAllArticlesResponseDto>>> GetAllArticles(int emailAccountId, int blogId) {
    var articles = await _context.Articles
      .Where(article => article.BlogId == blogId)
      .Select(article => new GetAllArticlesResponseDto {
        ArticleId = article.ArticleId,
        ArticleTitle = article.ArticleTitle,
        ArticleAuthor = article.ArticleAuthor,
        ArticleStatus = article.ArticleStatus,
        CreatedTimestamp = article.CreatedTimestamp,
        UpdatedTimestamp = article.UpdatedTimestamp,
        PublishedTimestamp = article.PublishedTimestamp,
        ArticleViewsCount = article.ArticleViewsCount
      })
      .ToListAsync();

    if(articles == null || !articles.Any())
    {
      return NotFound();
    }

    return Ok(articles);
  }

  /// <summary>
  /// Deletes an article by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="blogId">The ID of the blog.</param>
  /// <param name="articleId">The ID of the article to delete.</param>
  [HttpDelete("article/{articleId}/delete-article")]
  public async Task<IActionResult> DeleteArticleById(int emailAccountId, int blogId, int articleId)
  {
    var article = await _context.Articles
        .Where(a => a.ArticleId == articleId && a.BlogId == blogId)
        .FirstOrDefaultAsync();

    if(article == null)
    {
      return NotFound();
    }

    _context.Articles.Remove(article);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Updates an article's details by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account.</param>
  /// <param name="blogId">The ID of the blog.</param>
  /// <param name="articleId">The ID of the article to update.</param>
  /// <param name="articleUpdateDto">The DTO containing updated article details.</param>
  [HttpPut("article/{articleId}/update-article")]
  public async Task<IActionResult> UpdateArticle(int emailAccountId, int blogId, int articleId, ArticleUpdateDto articleUpdateDto)
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var article = await _context.Articles
        .Where(a => a.ArticleId == articleId && a.BlogId == blogId)
        .FirstOrDefaultAsync();

    if(article == null) {
      return NotFound();
    }

    article.ArticleTitle = articleUpdateDto.ArticleTitle;
    article.ArticleAuthor = articleUpdateDto.ArticleAuthor;
    article.Content = articleUpdateDto.Content;

    _context.Entry(article).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
  }

[HttpPut("article/{articleId}/publish")]
public async Task<IActionResult> PublishArticle(int blogId, int articleId)
{
  try
  {
    var article = await _context.Articles
        .Where(a => a.BlogId == blogId && a.ArticleId == articleId)
        .FirstOrDefaultAsync();

    if (article == null)
    {
        return NotFound();
    }

    // Update article properties based on the DTO
    article.ArticleStatus = "Published";
    article.PublishedTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");

    _context.Entry(article).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
  }
  catch (Exception ex)
  {
    return StatusCode(500, $"Internal server error: {ex.Message}");
  }
}

[HttpPut("article/{articleId}/to-drafts")]
public async Task<IActionResult> ToDraftArticle(int blogId, int articleId)
{
  try
  {
    var article = await _context.Articles
        .Where(a => a.BlogId == blogId && a.ArticleId == articleId)
        .FirstOrDefaultAsync();

    if (article == null)
    {
        return NotFound();
    }

    // Update article properties based on the DTO
    article.ArticleStatus = "Draft";

    _context.Entry(article).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
  }
  catch (Exception ex)
  {
    return StatusCode(500, $"Internal server error: {ex.Message}");
  }
}
}