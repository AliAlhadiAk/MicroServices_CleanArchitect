using MassTransit.Caching;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data.Repos.Caching;

namespace PlatformService.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> _logger;
		private readonly ICacheService _cacheService;

		public WeatherForecastController(ILogger<WeatherForecastController> logger, ICacheService cacheService)
		{
			_logger = logger;
			_cacheService = cacheService;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<IActionResult> Get()
		{
			 _cacheService.SetData<List<string>>("Key", new List<string> { "Ali", "Sara" }, TimeSpan.FromHours(1));
			var getRedis =  _cacheService.GetData<List<string>>("Key");
			return Ok(getRedis);
		}
	}
}
