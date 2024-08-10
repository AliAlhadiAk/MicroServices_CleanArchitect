using AutoMapper;
using MediatR;
using PlatformService.Data.Repos;
using PlatformService.Queries;
using PlatformService.Services.RabbitMq_MassTransit;

namespace PlatformService.Handlers
{
    public class DeletePlatformByIdHandler : IRequestHandler<DeletePlatformByIdQuery, bool>
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepo _platformRepo;
        private readonly IDriverNotificationPublisherService _driverNotification;

        public DeletePlatformByIdHandler(IMapper mapper, IPlatformRepo platformRepo, IDriverNotificationPublisherService driverNotification)
        {
            _mapper = mapper;
            _platformRepo = platformRepo;
            _driverNotification = driverNotification;
        }
        public async Task<bool> Handle(DeletePlatformByIdQuery request, CancellationToken cancellationToken)
        {
            var paltform = await _platformRepo.DeletePlatform(request.Id);
            if (paltform == false)
                return false;
            await _driverNotification.SendNotification("PlatformDeleted", paltform.ToString());
            return true;
        }
    }
}
