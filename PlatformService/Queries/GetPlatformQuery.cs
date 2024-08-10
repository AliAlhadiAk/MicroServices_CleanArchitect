using MediatR;
using PlatformService.Dto_s;

namespace PlatformService.Queries
{
	public class GetPlatformQuery : IRequest<PlatformReadDto>
	{
		public int PlatformId {  get; set; }
        public GetPlatformQuery(int PlatformID)
        {
            PlatformId = PlatformID;
        }
    }
}
