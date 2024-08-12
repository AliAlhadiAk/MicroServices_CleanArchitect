using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Commands;
using PlatformService.Data.Repos;
using PlatformService.Dto_s;
using PlatformService.Exceptions;
using PlatformService.Queries;
using PlatformService.Services.RabbitMq_MassTransit;

namespace PlatformService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformController : ControllerBase
	{

		private readonly IMediator _mediator;
		private readonly IPlatformRepo _platformService;

        public PlatformController(IMediator mediator, IPlatformRepo platformRepo)
        {
			_mediator = mediator;
            _platformService = platformRepo;
        }


		[HttpGet]
		public async Task<IActionResult> GetAllPlatforms()
		{
			Console.WriteLine("--> Getting Platforms....");

            var query = new GetAllPlatformsQuery();
            var result = await _mediator.Send(query);
            
            return Ok(result);
        }

		[HttpGet("{id}", Name = "GetPlatformById")]
		public async Task<IActionResult> GetPlatformById(int id)
		{
			try
			{
				var query = new GetPlatformQuery(id);
                var result = await _mediator.Send(query);
				return Ok(result);
			}

             catch(NotFoundException ex) 

			 { 
				return NotFound(new { Message = ex.Message });
			 }

			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
			}
		}

		[HttpPost]

		public async Task<IActionResult> CreatePlatform(PlatformCreateDto platformCreateDto)
		{
			var command = new CreatePlatformInfoRequest(platformCreateDto);
			var result = await _mediator.Send(command);
			return CreatedAtRoute(nameof(GetPlatformById), new { Id = result.Id }, result);
		}

		[HttpDelete("{Id:int}")]

        public async Task<IActionResult> DeletePlatformById(int Id)
        {
            var query = new DeletePlatformByIdQuery(Id);
             await _mediator.Send(query);

            return Ok("Platform Deleted Succesfully");

        }
    }
}
