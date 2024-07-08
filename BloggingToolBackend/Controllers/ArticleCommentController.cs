using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloggingTool.Models;
using BloggingTool.Context;
using BloggingTool.DTOs;

[ApiController]
[Route("controller")]
public class ArticleCommentController : ControllerBase
{
  private readonly AppDbContext _context;

  public ArticleCommentController(AppDbContext context)
  {
    _context = context;
  }

  
}