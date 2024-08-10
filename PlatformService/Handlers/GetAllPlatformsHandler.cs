using AutoMapper;
using MediatR;
using PlatformService.Data.Repos;
using PlatformService.Dto_s;
using PlatformService.Queries;
using PlatformService.Services.RabbitMq_MassTransit;

namespace PlatformService.Handlers
{
	public class GetAllPlatformsHandler : IRequestHandler<GetAllPlatformsQuery, IEnumerable<PlatformReadDto>>
	{
		private readonly IMapper _mapper;
		private readonly IPlatformRepo _platformRepo;
		private readonly IDriverNotificationPublisherService _driverNotification;
		private readonly ILogger<GetAllPlatformsHandler> _logger;

		public GetAllPlatformsHandler(IMapper mapper, IPlatformRepo platformRepo, IDriverNotificationPublisherService driverNotification, ILogger<GetAllPlatformsHandler> logger)
		{
			_mapper = mapper;
			_platformRepo = platformRepo;
			_driverNotification = driverNotification;
			_logger = logger;
		}
		public async Task<IEnumerable<PlatformReadDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
		{
			var platforms = await _platformRepo.GetAllPlatforms();
			await _driverNotification.SendNotification("SomeOne GetPlatfomrs", "hhhhhh");
			_logger.LogInformation("RabbitMq eunnnibg...");
			
			return _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
		}
	}
}
