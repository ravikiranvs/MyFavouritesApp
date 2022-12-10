using Microsoft.AspNetCore.Mvc;
using MyFavouritesEntities;
using Microsoft.AspNetCore.Authorization;

namespace MyFavouritesAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly AWSMySQL dbContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, AWSMySQL dbContext)
    {
        _logger = logger;
        this.dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var users = dbContext.Users.ToArray();
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

