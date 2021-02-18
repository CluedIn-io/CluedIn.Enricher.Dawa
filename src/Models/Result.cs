using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class Result
    {
        [JsonProperty("adresse")]
        public Address Address { get; set; }

        [JsonProperty("aktueladresse")]
        public ActualAddress ActualAddress { get; set; }

        [JsonProperty("vaskeresultat")]
        public WashResult WashResult { get; set; }
    }
}
