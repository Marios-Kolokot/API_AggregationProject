using API_AggregationProject.Config;
using API_AggregationProject.Services;
using API_AggregationProject.Services.ExternalApiClients;
using API_AggregationProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var newsApiKey = Environment.GetEnvironmentVariable("NEWSAPI_KEY");
builder.Services.AddSingleton(new NewsApiConfig { ApiKey = newsApiKey });

builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<OpenWeatherApiClient>();
builder.Services.AddHttpClient<NewsApiClient>();
builder.Services.AddHttpClient<GithubApiClient>();

builder.Services.AddScoped<IExternalApiClient, OpenWeatherApiClient>();
builder.Services.AddScoped<IExternalApiClient, NewsApiClient>();
builder.Services.AddScoped<IExternalApiClient, GithubApiClient>();

builder.Services.AddScoped<IApiAggregatorService, ApiAggregatorService>();
builder.Services.AddSingleton<IStatisticsService, StatisticsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.UseHttpsRedirection();

//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", () =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//});

app.Run();

//internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
