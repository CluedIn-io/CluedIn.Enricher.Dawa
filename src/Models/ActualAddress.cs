using Newtonsoft.Json;
using System;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class ActualAddress
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("vejnavn")]
        public string StreetName { get; set; }

        [JsonProperty("adresseringsvejnavn")]
        public string AdressingStreetName { get; set; }

        [JsonProperty("husnr")]
        public string HouseNumber { get; set; }

        [JsonProperty("supplerendebynavn")]
        public object AdditionalCityName { get; set; }

        [JsonProperty("postnr")]
        public string Zipcode { get; set; }

        [JsonProperty("postnrnavn")]
        public string Zipcodename { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("virkningstart")]
        public DateTime EffectiveStart { get; set; }

        [JsonProperty("virkningslut")]
        public DateTime? EffectiveEnd { get; set; }

        [JsonProperty("adgangsadresseid")]
        public string AccessAddressId { get; set; }

        [JsonProperty("etage")]
        public string Floor { get; set; }

        [JsonProperty("dør")]
        public object Door { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

}
