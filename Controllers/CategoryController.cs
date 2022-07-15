using Blog.Data;
using Blog.DTOs;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices]BlogDataContext context)
        => Ok(await context.Categories.ToListAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromServices]BlogDataContext context, [FromRoute] int id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x=> x.Id == id);
            if(category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices]BlogDataContext context, [FromBody]CreateCategoryDTO category)
        {   
            if(category == null)
                return BadRequest($"{nameof(category)} cannot be null.");
            try
            {
                var categoria = new Category()
                {
                    Id = 0,
                    Name = category.Name,
                    Slug = category.Slug.ToLower(),
                };
                await context.Categories.AddAsync(categoria);
                await context.SaveChangesAsync();
                return Created($"api/v1/category/{categoria.Id}", categoria);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Não foi possível inserir a categoria");
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha no servidor");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices]BlogDataContext context, [FromRoute] int id, [FromBody]Category category)
        {   
            var categoria = await context.Categories.FirstOrDefaultAsync(x=> x.Id == id);
            if(categoria == null)
                return NotFound($"{nameof(categoria)} not found");
            try
            {
                categoria!.Name = category.Name;
                categoria.Slug = category.Slug;
                context.Categories.Update(categoria);
                await context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "05XE9 Não foi possível atualizar a categoria");
            }
            catch (Exception)
            {
                return StatusCode(500, "Falha no servidor");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromServices]BlogDataContext context, [FromRoute] int id)
        {   
            var categoria = await context.Categories.FirstOrDefaultAsync(x=> x.Id == id);
            if(categoria == null)
                return NotFound($"{nameof(categoria)} not found");
            try
            {
                context.Categories.Remove(categoria);
                await context.SaveChangesAsync();
                return Ok(categoria);
            }
            catch (Exception)
            {
                return BadRequest("Não foi possível cadastrar");
            }
        }
    }
}