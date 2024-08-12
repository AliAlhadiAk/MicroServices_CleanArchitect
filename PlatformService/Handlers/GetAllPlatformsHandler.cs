using AutoMapper;
using MediatR;
using PlatformService.Data.Repos;
using PlatformService.Dto_s;
using PlatformService.Models;
using PlatformService.Queries;
using PlatformService.Services.RabbitMq_MassTransit;
using Microsoft.Extensions.Logging; // Ensure this namespace is included
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformService.Handlers
{
    public class GetAllPlatformsHandler : IRequestHandler<GetAllPlatformsQuery, IEnumerable<PlatformReadDto>>
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;
        private readonly IDriverNotificationPublisherService _driverNotification;
        private readonly ILogger<GetAllPlatformsHandler> _logger;

        public GetAllPlatformsHandler(
            IMapper mapper,
            IPlatformRepo platformRepo,
            IDriverNotificationPublisherService driverNotification,
            ILogger<GetAllPlatformsHandler> logger)
        {
            _mapper = mapper;
            _platformRepo = platformRepo;
            _driverNotification = driverNotification;
            _logger = logger;
        }

        public async Task<IEnumerable<PlatformReadDto>> Handle(GetAllPlatformsQuery request, CancellationToken cancellationToken)
        {
            var platforms = await _platformRepo.GetAllPlatforms();
            await _driverNotification.SendNotification("Someone GetPlatforms", "Notification message");
            _logger.LogInformation("RabbitMq is running...");
            var platformReadDto = _mapper.Map<IEnumerable<PlatformReadDto>>(platforms);
            return platformReadDto;
        }
    }
}
