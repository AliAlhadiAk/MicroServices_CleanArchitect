using MediatR;
using PlatformService.Dto_s;

namespace PlatformService.Commands
{
	public class CreatePlatformInfoRequest : IRequest<PlatformReadDto>
	{
		public PlatformCreateDto CreatePlatform { get; set; }

        public CreatePlatformInfoRequest(PlatformCreateDto CreatePlatformDTO)
        {
			CreatePlatform = CreatePlatformDTO;

		}
    }
}
