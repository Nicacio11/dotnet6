using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.DTOs;
using Blog.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly BlogDataContext context;

        public PostController(ILogger<PostController> logger, BlogDataContext context)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery]int page = 0, [FromQuery]int pageSize = 20)
        {
            try
            {
                return Ok(await context.Posts
                .Include(x => x.Author)
                .Include(x => x.Category)
                // .Select(p => new ListPostsDTO
                // {
                //     Id = p.Id, 
                //     Title = p.Title,
                //     LastUpdateDate = p.LastUpdateDate,
                //     Slug = p.Slug,
                //     Author = p.Author.Name,
                //     Category = p.Category.Name
                // })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.LastUpdateDate)
                .ToListAsync());
                
            }
            catch
            {
                return StatusCode(500, new ResultDTO<string>(error: "Falha interna"));
            } 
        }
    }
}