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

		public GetAllPlatformsHandler(IMapper mapper, IPlatformRepo platformRepo, IDriverNotificationPublisherService driverNotification)
		{
			_mapper = mapper;
			_platformRepo = platformRepo;
			_driverNotification = driverNotification;
		}
		public async Task<IEnumerable<PlatformReadDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
		{
			var platforms = await _platformRepo.GetAllPlatforms();
			return _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
		}
	}
}
