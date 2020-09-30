using Newtonsoft.Json;
using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class WashResult
    {

        [JsonProperty("variant")]
        public Variant Variant { get; set; }

        [JsonProperty("afstand")]
        public int Distance { get; set; }

        [JsonProperty("forskelle")]
        public Differences Differences { get; set; }

        [JsonProperty("parsetadresse")]
        public ParsedAddress ParsedAddress { get; set; }

        [JsonProperty("ukendtetokens")]
        public List<string> UnknownTokens { get; set; }

        [JsonProperty("anvendtstormodtagerpostnummer")]
        public object AppliedMajorRecipientZipcode { get; set; }
    }

}
