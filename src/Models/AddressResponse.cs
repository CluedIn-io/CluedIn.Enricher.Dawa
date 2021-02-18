using Newtonsoft.Json;
using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class AddressResponse
    {

        [JsonProperty("kategori")]
        public string Category { get; set; }

        [JsonProperty("resultater")]
        public List<Result> Results { get; set; }
    }


}
