using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class Differences
    {

        [JsonProperty("vejnavn")]
        public int StreetName { get; set; }

        [JsonProperty("husnr")]
        public int HouseNumber { get; set; }

        [JsonProperty("postnr")]
        public int Zipcode { get; set; }

        [JsonProperty("postnrnavn")]
        public int ZipcodeName { get; set; }

        [JsonProperty("etage")]
        public int? Floor { get; set; }
    }

}
