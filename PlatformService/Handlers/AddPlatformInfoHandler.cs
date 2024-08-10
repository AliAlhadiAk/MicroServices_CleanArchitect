using AutoMapper;
using MediatR;
using PlatformService.Commands;
using PlatformService.Data.Repos;
using PlatformService.Dto_s;
using PlatformService.Services.RabbitMq_MassTransit;
using System.Threading.Tasks;

namespace PlatformService.Handlers
{
    public class AddPlatformInfoHandler : IRequestHandler<CreatePlatformInfoRequest, PlatformReadDto>
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;
        private readonly IDriverNotificationPublisherService _driverNotification;

        public AddPlatformInfoHandler(IMapper mapper, IPlatformRepo platformRepo, IDriverNotificationPublisherService driverNotification)
        {
            _mapper = mapper;
            _platformRepo = platformRepo;
            _driverNotification = driverNotification;
        }

        public async Task<PlatformReadDto> Handle(CreatePlatformInfoRequest request, CancellationToken cancellationToken)
        {
            if (request.CreatePlatform == null)
            {
                throw new ArgumentNullException(nameof(request.CreatePlatform), "Platform creation data cannot be null.");
            }

            try
            {
                var platformModel = _mapper.Map<Models.Platfrom>(request.CreatePlatform);
                await _platformRepo.CreatePlatform(platformModel);
                await _platformRepo.SaveChanges();

                var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

                await _driverNotification.SendNotification(platformReadDto.Name, platformReadDto.Publisher);

                return platformReadDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while handling CreatePlatformInfoRequest: {ex.Message}");
                throw; 
            }
        }
    }
}
