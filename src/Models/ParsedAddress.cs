using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class ParsedAddress
    {

        [JsonProperty("vejnavn")]
        public string StreetName { get; set; }

        [JsonProperty("husnr")]
        public string HouseNumber { get; set; }

        [JsonProperty("postnr")]
        public string Zipcode { get; set; }

        [JsonProperty("postnrnavn")]
        public string ZipcodeName { get; set; }

        [JsonProperty("etage")]
        public string Floor { get; set; }
    }

}
