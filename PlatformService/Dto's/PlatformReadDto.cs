using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlatformService.Dto_s
{
	public class PlatformReadDto
	{
	
		public int Id { get; set; }
        [JsonIgnore]

        public string Name { get; set; }
        [JsonIgnore]

        public string Publisher { get; set; }
        [JsonIgnore]

        public string Cost { get; set; }
	}
}
