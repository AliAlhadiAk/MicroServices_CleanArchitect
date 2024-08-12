using MediatR;
using PlatformService.Dto_s;
using System.ComponentModel;

namespace PlatformService.Queries
{
	public class GetAllPlatformsQuery : IRequest<IEnumerable<PlatformReadDto>>
	{
	}
}
