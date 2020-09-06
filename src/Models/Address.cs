using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CluedIn.ExternalSearch.Providers.Dawa.Models
{
    public class Adresse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("vejnavn")]
        public string Vejnavn { get; set; }

        [JsonProperty("adresseringsvejnavn")]
        public string Adresseringsvejnavn { get; set; }

        [JsonProperty("husnr")]
        public string Husnr { get; set; }

        [JsonProperty("supplerendebynavn")]
        public object Supplerendebynavn { get; set; }

        [JsonProperty("postnr")]
        public string Postnr { get; set; }

        [JsonProperty("postnrnavn")]
        public string Postnrnavn { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("virkningstart")]
        public DateTime Virkningstart { get; set; }

        [JsonProperty("virkningslut")]
        public DateTime? Virkningslut { get; set; }

        [JsonProperty("adgangsadresseid")]
        public string Adgangsadresseid { get; set; }

        [JsonProperty("etage")]
        public string Etage { get; set; }

        [JsonProperty("dør")]
        public object Dør { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class Aktueladresse
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("vejnavn")]
        public string Vejnavn { get; set; }

        [JsonProperty("adresseringsvejnavn")]
        public string Adresseringsvejnavn { get; set; }

        [JsonProperty("husnr")]
        public string Husnr { get; set; }

        [JsonProperty("supplerendebynavn")]
        public object Supplerendebynavn { get; set; }

        [JsonProperty("postnr")]
        public string Postnr { get; set; }

        [JsonProperty("postnrnavn")]
        public string Postnrnavn { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("virkningstart")]
        public DateTime Virkningstart { get; set; }

        [JsonProperty("virkningslut")]
        public object Virkningslut { get; set; }

        [JsonProperty("adgangsadresseid")]
        public string Adgangsadresseid { get; set; }

        [JsonProperty("etage")]
        public string Etage { get; set; }

        [JsonProperty("dør")]
        public object Dør { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

    public class Variant
    {

        [JsonProperty("vejnavn")]
        public string Vejnavn { get; set; }

        [JsonProperty("husnr")]
        public string Husnr { get; set; }

        [JsonProperty("etage")]
        public string Etage { get; set; }

        [JsonProperty("dør")]
        public object Dør { get; set; }

        [JsonProperty("supplerendebynavn")]
        public object Supplerendebynavn { get; set; }

        [JsonProperty("postnr")]
        public string Postnr { get; set; }

        [JsonProperty("postnrnavn")]
        public string Postnrnavn { get; set; }
    }

    public class Forskelle
    {

        [JsonProperty("vejnavn")]
        public int Vejnavn { get; set; }

        [JsonProperty("husnr")]
        public int Husnr { get; set; }

        [JsonProperty("postnr")]
        public int Postnr { get; set; }

        [JsonProperty("postnrnavn")]
        public int Postnrnavn { get; set; }

        [JsonProperty("etage")]
        public int? Etage { get; set; }
    }

    public class Parsetadresse
    {

        [JsonProperty("vejnavn")]
        public string Vejnavn { get; set; }

        [JsonProperty("husnr")]
        public string Husnr { get; set; }

        [JsonProperty("postnr")]
        public string Postnr { get; set; }

        [JsonProperty("postnrnavn")]
        public string Postnrnavn { get; set; }

        [JsonProperty("etage")]
        public string Etage { get; set; }
    }

    public class Vaskeresultat
    {

        [JsonProperty("variant")]
        public Variant Variant { get; set; }

        [JsonProperty("afstand")]
        public int Afstand { get; set; }

        [JsonProperty("forskelle")]
        public Forskelle Forskelle { get; set; }

        [JsonProperty("parsetadresse")]
        public Parsetadresse Parsetadresse { get; set; }

        [JsonProperty("ukendtetokens")]
        public List<string> Ukendtetokens { get; set; }

        [JsonProperty("anvendtstormodtagerpostnummer")]
        public object Anvendtstormodtagerpostnummer { get; set; }
    }

    public class Resultater
    {

        [JsonProperty("adresse")]
        public Adresse Adresse { get; set; }

        [JsonProperty("aktueladresse")]
        public Aktueladresse Aktueladresse { get; set; }

        [JsonProperty("vaskeresultat")]
        public Vaskeresultat Vaskeresultat { get; set; }
    }

    public class AddressResponse
    {

        [JsonProperty("kategori")]
        public string Kategori { get; set; }

        [JsonProperty("resultater")]
        public List<Resultater> Resultater { get; set; }
    }


}
