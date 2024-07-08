using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("controller")]
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
  /// <param name="blogDto">The blog DTO containing blog details.</param>
  /// <returns>
  /// Returns the created blog details without the articles collection.
  /// If the model state is invalid, returns a 400 Bad Request response.
  /// If the blog is successfully created, returns a 201 Created response with the blog details.
  /// </returns>
  [HttpPost("add-blog")]
  public async Task<IActionResult> AddBlog(BlogDto blogDto)
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
      EmailAccountId = blogDto.EmailAccountid
    };

    var response = new BlogResponseDto
    {
        BlogId = blog.BlogId,
        BlogTitle = blog.BlogTitle,
        BlogAuthor = blog.BlogAuthor,
        BlogCategory = blog.BlogCategory,
        EmailAccountId = blog.EmailAccountId
    };

    _context.Blogs.Add(blog);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetBlogById), new { id = blog.BlogId}, response);
  }

  /// <summary>
  /// Retrieves a blog by its ID.
  /// </summary>
  /// <param name="id">The ID of the blog to retrieve.</param>
  [HttpGet("get-blog/{id}")]
  public async Task<ActionResult<Blog>> GetBlogById(int id)
  {
    var blog = await _context.Blogs.FindAsync(id);

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
  [HttpGet("get-all-blogs/{emailAccountId}")]
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
  /// <param name="id">The ID of the blog to delete.</param>
  [HttpDelete("delete-blog/{id}")]
  public async Task<IActionResult> DeleteBlogById(int id)
  {
    var blog = await _context.Blogs.FindAsync(id);

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
  /// <param name="id">The ID of the blog to update.</param>
  /// <param name="blogUpdateDto">The DTO containing updated blog details.</param>
  [HttpPut("edit-blog/{id}")]
  public async Task<IActionResult> UpdateBlog(int id, BlogUpdateDto blogUpdateDto)
  {
    if(!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var blog = await _context.Blogs.FindAsync(id);

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