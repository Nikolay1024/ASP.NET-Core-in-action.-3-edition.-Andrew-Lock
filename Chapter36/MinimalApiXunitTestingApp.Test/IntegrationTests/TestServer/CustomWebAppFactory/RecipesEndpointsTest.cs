using MinimalApiXunitTestingApp.Commands;
using MinimalApiXunitTestingApp.ViewModels;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.CustomWebAppFactory
{
    public class RecipesEndpointsTest : IClassFixture<CustomWebAppFactory>
    {
        private readonly CustomWebAppFactory _fixture;

        public RecipesEndpointsTest(CustomWebAppFactory fixture) => _fixture = fixture;

        [Fact]
        public async Task PostGetRecipes_UsualCase()
        {
            HttpClient httpClient = _fixture.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(new RecipeCreateCommand()
            {
                Name = "Пельмени",
                Method = "Варить",
                TimeToCookMins = 20,
                Ingredients = new List<IngredientCreateCommand>()
                {
                    new() { Name = "Пельмени", Quantity = 20, Unit = "шт" },
                    new() { Name = "Вода", Quantity = 2, Unit = "литр" },
                }
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

            HttpResponseMessage response1 = await httpClient.PostAsync("recipes", requestContent);
            response1.EnsureSuccessStatusCode();

            HttpResponseMessage response2 = await httpClient.GetAsync("recipes/1");
            response2.EnsureSuccessStatusCode();
            RecipeViewModel? actual = await response2.Content.ReadFromJsonAsync<RecipeViewModel?>();

            var expected = new RecipeViewModel()
            {
                Id = 1,
                Name = "Пельмени",
                Method = "Варить",
                Ingredients = new List<IngredientViewModel>()
                {
                    new() { Name = "Пельмени", Quantity = 20, Unit = "шт" },
                    new() { Name = "Вода", Quantity = 2, Unit = "литр" },
                }
            };
            Assert.Equivalent(expected, actual);
        }
    }
}
