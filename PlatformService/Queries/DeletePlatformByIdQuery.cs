using MediatR;

namespace PlatformService.Queries
{
    public class DeletePlatformByIdQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public DeletePlatformByIdQuery(int PlatformId)
        {
            PlatformId = this.Id;
        }
    }
}
