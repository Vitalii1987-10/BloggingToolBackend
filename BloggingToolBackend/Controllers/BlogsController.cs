using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("user/{emailAccountId}/")]
public class BlogsController : ControllerBase 
{
  private readonly AppDbContext _context;

  public BlogsController(AppDbContext context) 
  {
    _context = context;
  }


  /// <summary>
  /// Adds a new blog.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account associated with the blog.</param>
  /// <param name="blogDto">The blog DTO containing blog details.</param>
  /// <returns>
  /// Returns the created blog details without the articles collection.
  /// If the model state is invalid, returns a 400 Bad Request response.
  /// If the blog is successfully created, returns a 201 Created response with the blog details.
  /// </returns>
  [HttpPost("add-blog")]
  public async Task<IActionResult> AddBlog(int emailAccountId, BlogDto blogDto)
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var blog = new Blog 
    {
      BlogId = blogDto.BlogId,
      BlogTitle = blogDto.BlogTitle,
      BlogAuthor = blogDto.BlogAuthor,
      BlogCategory = blogDto.BlogCategory,
      EmailAccountId = emailAccountId
    };

    var response = new BlogResponseDto
    {
        BlogId = blog.BlogId,
        BlogTitle = blog.BlogTitle,
        BlogAuthor = blog.BlogAuthor,
        BlogCategory = blog.BlogCategory,
    };

    _context.Blogs.Add(blog);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetBlogById), new { emailAccountId = blog.EmailAccountId, blogId = blog.BlogId}, response);
  }

  /// <summary>
  /// Retrieves a blog by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account associated with the blog.</param>
  /// <param name="blogId">The ID of the blog to retrieve.</param>
  /// <returns>The blog details.</returns>
  [HttpGet("blog/{blogId}")]
  public async Task<ActionResult<Blog>> GetBlogById(int emailAccountId, int blogId)
  {
      var blog = await _context.Blogs
          .Where(b => b.EmailAccountId == emailAccountId && b.BlogId == blogId)
          .FirstOrDefaultAsync();

      if(blog == null)
      {
          return NotFound();
      }

      return blog;
  }

/// <summary>
/// Retrieves all blogs associated with a specific email account.
/// </summary>
/// <param name="emailAccountId">The ID of the email account.</param>
/// <returns>
/// Returns a list of blogs associated with the specified email account.
/// If no blogs are found, returns a 404 Not Found response.
/// </returns>
  [HttpGet("blogs")]
  public async Task<ActionResult<IEnumerable<Blog>>> GetAllBlogs(int emailAccountId)
  {
    var blogs = await _context.Blogs
      .Where(blog => blog.EmailAccountId == emailAccountId)
      .ToListAsync();

      if(blogs == null || !blogs.Any())
      {
        return NotFound();
      }

      return blogs;
  }

  /// <summary>
  /// Deletes a blog by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account associated with the blog.</param>
  /// <param name="blogId">The ID of the blog to delete.</param>
  /// <returns>No content if the deletion is successful; otherwise, returns a 404 Not Found response.</returns>
  [HttpDelete("delete-blog/{blogId}")]
  public async Task<IActionResult> DeleteBlogById(int emailAccountId, int blogId)
  {
    // var blog = await _context.Blogs.FindAsync(id);

      var blog = await _context.Blogs
        .Where(b => b.EmailAccountId == emailAccountId && b.BlogId == blogId)
        .FirstOrDefaultAsync();

    if(blog == null)
    {
      return NotFound();
    }

    _context.Blogs.Remove(blog);
    await _context.SaveChangesAsync();

    return NoContent();
  }

   /// <summary>
  /// Updates a blog's details by its ID.
  /// </summary>
  /// <param name="emailAccountId">The ID of the email account associated with the blog.</param>
  /// <param name="blogId">The ID of the blog to update.</param>
  /// <param name="blogUpdateDto">The DTO containing updated blog details.</param>
  /// <returns>No content if the update is successful; otherwise, returns a 400 Bad Request response if the model state is invalid, or a 404 Not Found response if the blog does not exist.</returns>
  [HttpPut("update-blog/{blogId}")]
  public async Task<IActionResult> UpdateBlog(int emailAccountId, int blogId, BlogUpdateDto blogUpdateDto)
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    // var blog = await _context.Blogs.FindAsync(id);

      var blog = await _context.Blogs
        .Where(b => b.EmailAccountId == emailAccountId && b.BlogId == blogId)
        .FirstOrDefaultAsync();

    if(blog == null) {
      return NotFound();
    }
    
    blog.BlogTitle = blogUpdateDto.BlogTitle;
    blog.BlogAuthor = blogUpdateDto.BlogAuthor;
    blog.BlogCategory = blogUpdateDto.BlogCategory;

    _context.Entry(blog).State = EntityState.Modified;
    await _context.SaveChangesAsync();

     return NoContent();
  }
}