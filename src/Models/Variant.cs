using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class Variant
    {

        [JsonProperty("vejnavn")]
        public string StreeName { get; set; }

        [JsonProperty("husnr")]
        public string HouseNumber { get; set; }

        [JsonProperty("etage")]
        public string Floor { get; set; }

        [JsonProperty("dør")]
        public object Door { get; set; }

        [JsonProperty("supplerendebynavn")]
        public object AdditionalCityName { get; set; }

        [JsonProperty("postnr")]
        public string Zipcode { get; set; }

        [JsonProperty("postnrnavn")]
        public string ZipcodeName { get; set; }
    }

}
