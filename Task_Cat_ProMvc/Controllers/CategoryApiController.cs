
using Microsoft.AspNetCore.Mvc;
using Task_Cat_ProMvc.Models.Data;
using Task_Cat_ProMvc.Services;

namespace Task_Cat_ProMvc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly ICategory _categoryService;

        public CategoryApiController(ICategory categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(int pageNumber = 1, int pageSize = 10)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<Category> { Categories = categories });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _categoryService.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _categoryService.UpdateCategoryAsync(id, category);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DaleteCategoryAsyn(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}

