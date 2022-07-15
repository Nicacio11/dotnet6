using Blog.Data;
using Blog.DTOs;
using Blog.Extensions;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices]BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ListCategoryResultDTO(categories));
            }
            catch
            {
                return StatusCode(500, new ListCategoryResultDTO("Falha na consulta de dados"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromServices]BlogDataContext context, [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x=> x.Id == id);
                if(category == null)
                    return NotFound(new CategoryResultDTO("Catégoria não encontrada"));

                return Ok(new CategoryResultDTO(category));
            }
            catch
            {
                return StatusCode(500, new CategoryResultDTO("Falha na consulta de dados"));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices]BlogDataContext context, [FromBody]EditorCategoryDTO category)
        {   
            if(category == null)
                return BadRequest($"{nameof(category)} cannot be null.");
            if(!ModelState.IsValid)
                return BadRequest(new CategoryResultDTO(ModelState.GetErrors()));
            
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
                return StatusCode(500, new CategoryResultDTO("Não foi possível inserir a categoria"));
            }
            catch
            {
                return StatusCode(500, new CategoryResultDTO("Falha no servidor"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync([FromServices]BlogDataContext context, [FromRoute] int id, [FromBody]EditorCategoryDTO category)
        {   
            if(!ModelState.IsValid)
                return BadRequest(new CategoryResultDTO(ModelState.GetErrors()));
            
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
                return StatusCode(500, new CategoryResultDTO("Não foi possível inserir a categoria"));
            }
            catch
            {
                return StatusCode(500, new CategoryResultDTO("Falha no servidor"));
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