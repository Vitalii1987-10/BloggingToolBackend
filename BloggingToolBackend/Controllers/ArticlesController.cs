using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("[controller]")]
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
  /// <param name="blogId">The ID of the blog to which the article will be added.</param>
  /// <param name="articleDto">The article DTO containing article details.</param>
  [HttpPost("add-article/{blogId}")]
  public async Task<IActionResult> AddArticle(int blogId, ArticleDto articleDto)
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
      CreatedTimestamp = articleDto.CreatedTimestamp,
      ArticleViewsCount = articleDto.ArticleViewsCount,
      Content = articleDto.Content,
      BlogId = articleDto.BlogId
    };

    var response = new ArticleOnCreateResponseDto {
      ArticleTitle = articleDto.ArticleTitle,
      ArticleAuthor = articleDto.ArticleAuthor,
      ArticleStatus = articleDto.ArticleStatus,
      CreatedTimestamp = articleDto.CreatedTimestamp,
      ArticleViewsCount = articleDto.ArticleViewsCount,
      Content = articleDto.Content,
      BlogId = articleDto.BlogId
    };

    _context.Articles.Add(article);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetArticleById), new { id = article.ArticleId}, response);
  }

  /// <summary>
  /// Retrieves an article by its ID.
  /// </summary>
  /// <param name="id">The ID of the article to retrieve.</param>
  [HttpGet("get-article/{id}")]
  public async Task<ActionResult<GetArticleByIdResponseDto>> GetArticleById(int id)
  {
    var article = await _context.Articles
        .Where(article => article.ArticleId == id)
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
  /// Retrieves all articles associated with a specific blog.
  /// </summary>
  /// <param name="blogId">The ID of the blog whose articles to retrieve.</param>
  [HttpGet("get-all-articles")]
  public async Task<ActionResult<IEnumerable<GetAllArticlesResponseDto>>> GetAllArticles(int blogId) {
    var articles = await _context.Articles
      .Where(article => article.BlogId == blogId)
      .Select(article => new GetAllArticlesResponseDto {
        ArticleId = article.ArticleId,
        ArticleTitle = article.ArticleTitle,
        ArticleAuthor = article.ArticleAuthor,
        CreatedTimestamp = article.CreatedTimestamp
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
  /// <param name="id">The ID of the article to delete.</param>
  [HttpDelete("delete-article/{id}")]
  public async Task<IActionResult> DeleteArticleById(int id)
  {
    var article = await _context.Articles.FindAsync(id);

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
  /// <param name="id">The ID of the article to update.</param>
  /// <param name="articleUpdateDto">The DTO containing updated article details.</param>
  [HttpPut("edit-article/id")]
  public async Task<IActionResult> UpdateArticle(int id, ArticleUpdateDto articleUpdateDto)
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var article = await _context.Articles.FindAsync(id);

    if(article == null) {
      return NotFound();
    }

    article.ArticleTitle = articleUpdateDto.ArticleTitle;
    article.ArticleAuthor = articleUpdateDto.ArticleAuthor;

    _context.Entry(article).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return NoContent();
  }
}