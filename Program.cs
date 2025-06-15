using ApiAggregator.Services.Implementations;
using ApiAggregator.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ApiStatsTracker>();

builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IGitHubService, GitHubService>();
builder.Services.AddScoped<IAggregatorService, AggregatorService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<RequestTimingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
