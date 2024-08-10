using AutoMapper;
using MediatR;
using PlatformService.Data.Repos;
using PlatformService.Dto_s;
using PlatformService.Exceptions;
using PlatformService.Queries;
using PlatformService.Services.RabbitMq_MassTransit;

public class GetPlatformHandler : IRequestHandler<GetPlatformQuery, PlatformReadDto>
{
	private readonly IMapper _mapper;
	private readonly IPlatformRepo _platformRepo;
	private readonly IDriverNotificationPublisherService _driverNotification;

	public GetPlatformHandler(IMapper mapper, IPlatformRepo platformRepo, IDriverNotificationPublisherService driverNotification)
	{
		_mapper = mapper;
		_platformRepo = platformRepo;
		_driverNotification = driverNotification;
	}

	public async Task<PlatformReadDto> Handle(GetPlatformQuery request, CancellationToken cancellationToken)
	{
		var platformItem = await _platformRepo.GetPlafromById(request.PlatformId);

		if (platformItem == null)
		{
		
			throw new NotFoundException("Platform not found");
		}

		// Map the platform item to the DTO
		var platformReadDto = _mapper.Map<PlatformReadDto>(platformItem);

		await _driverNotification.SendNotification(platformReadDto);
		return platformReadDto;
	}
}
