using Newtonsoft.Json;

namespace Festo.Config.Api.Model
{
    public class Motor
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        public string Description { get; set; }

        public float MaxCurrent { get; set; }
    }
}
