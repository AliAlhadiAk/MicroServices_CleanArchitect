using MediatR;
using PlatformService.Dto_s;

namespace PlatformService.Queries
{
	public class GetAllPlatformsQuery : IRequest<IEnumerable<PlatformReadDto>>
	{
	}
}
