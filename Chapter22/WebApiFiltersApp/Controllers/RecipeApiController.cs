using Microsoft.AspNetCore.Mvc;
using WebApiFiltersApp.Commands;
using WebApiFiltersApp.Services;
using WebApiFiltersApp.ViewModels;

namespace WebApiFiltersApp.Controllers
{
    [Route("api/recipe")]
    public class RecipeApiController : ControllerBase
    {
        readonly bool IsEnabled = true;
        readonly RecipeService _recipeService;

        public RecipeApiController(RecipeService recipeService) => _recipeService = recipeService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!IsEnabled)
                return BadRequest();

            try
            {
                if (!await _recipeService.DoesRecipeExist(id))
                    return NotFound();

                RecipeViewModel? recipe = await _recipeService.GetRecipeDetail(id);
                Response.GetTypedHeaders().LastModified = recipe!.LastModified;
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return GetErrorResponse(ex);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RecipeUpdateCommand command)
        {
            if (!IsEnabled)
                return BadRequest();

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!await _recipeService.DoesRecipeExist(id))
                    return NotFound();

                await _recipeService.UpdateRecipe(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return GetErrorResponse(ex);
            }
        }

        static IActionResult GetErrorResponse(Exception ex)
        {
            var problemDetails = new ProblemDetails()
            {
                Title = "An error occurred",
                Detail = ex.Message,
                Status = 500,
                Type = "https://httpstatuses.com/500",
            };

            return new ObjectResult(problemDetails) { StatusCode = 500, };
        }
    }
}
