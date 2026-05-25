using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiFiltersApp.Data;
using WebApiFiltersApp.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo() { Title = "Рецепты.", Version = "v1" }));

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString!));
builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
