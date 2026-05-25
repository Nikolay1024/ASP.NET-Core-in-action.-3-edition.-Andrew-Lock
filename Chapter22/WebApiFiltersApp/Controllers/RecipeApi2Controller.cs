using Microsoft.AspNetCore.Mvc;
using WebApiFiltersApp.Commands;
using WebApiFiltersApp.Filters.ActionFilters;
using WebApiFiltersApp.Filters.ExceptionFilters;
using WebApiFiltersApp.Filters.ResourceFilters;
using WebApiFiltersApp.Filters.ResultFilters;
using WebApiFiltersApp.Services;
using WebApiFiltersApp.ViewModels;

namespace WebApiFiltersApp.Controllers
{
    [ApiController, Route("api/recipe2")]
    // Применение пользовательских фильтров (ресурсов и обработки ошибок) к контроллеру.
    [FeatureEnabled(IsEnabled = true), HandleException]
    public class RecipeApi2Controller : ControllerBase
    {
        readonly RecipeService _recipeService;

        public RecipeApi2Controller(RecipeService recipeService) => _recipeService = recipeService;

        [HttpGet("{id}")]
        // Применение пользовательских фильтров (действия и результата) к методу действия.
        [EnsureRecipeExists2, AddLastModifedHeader]
        public async Task<IActionResult> Get(int id)
        {
            RecipeViewModel? recipe = await _recipeService.GetRecipeDetail(id);
            return Ok(recipe);
        }

        [HttpPost("{id}")]
        // Применение пользовательского фильтра (действия) к методу действия.
        [EnsureRecipeExists2]
        public async Task<IActionResult> Edit(int id, RecipeUpdateCommand command)
        {
            await _recipeService.UpdateRecipe(command);
            return Ok();
        }
    }
}
