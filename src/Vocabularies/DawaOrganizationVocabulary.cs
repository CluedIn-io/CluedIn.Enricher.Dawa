using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Dawa.Vocabularies
{
    public class DawaOrganizationVocabulary : DawaVocabulary
    {
        public DawaOrganizationVocabulary()
        {
            this.VocabularyName = "Dawa Organization";
            this.KeyPrefix = "dawa.organization";
            this.KeySeparator = ".";
            this.Grouping = CluedIn.Core.Data.EntityType.Organization;

            this.AddGroup("Address Data", group =>
                {
                    this.Virkningstart = group.Add(new VocabularyKey("Virkningstart", VocabularyKeyDataType.Number, VocabularyKeyVisibility.Hidden));
                    this.Virkningslut = group.Add(new VocabularyKey("Virkningslut"));
                    this.Vejnavn = group.Add(new VocabularyKey("Vejnavn"));
                    this.Supplerendebynavn = group.Add(new VocabularyKey("Supplerendebynavn"));
                    this.Status = group.Add(new VocabularyKey("Status"));
                    this.Postnrnavn = group.Add(new VocabularyKey("Postnrnavn", VocabularyKeyDataType.Uri));
                    this.Postnr = group.Add(new VocabularyKey("Postnr", VocabularyKeyDataType.Uri));
                    this.Id = group.Add(new VocabularyKey("Id", VocabularyKeyDataType.Uri));
                    this.Husnr = group.Add(new VocabularyKey("Husnr", VocabularyKeyDataType.Uri));
                    this.Etage = group.Add(new VocabularyKey("Etage", VocabularyKeyDataType.Uri));
                    this.Dør = group.Add(new VocabularyKey("Dør", VocabularyKeyDataType.Uri));
                    this.Adresseringsvejnavn = group.Add(new VocabularyKey("Adresseringsvejnavn", VocabularyKeyDataType.Uri));
                });

            this.AddMapping(this.Adresseringsvejnavn, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressStreetName);
            this.AddMapping(this.Husnr, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressStreetNumber);
            this.AddMapping(this.Postnr, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressZipCode);
            this.AddMapping(this.Postnrnavn, CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressPostalArea);
        }

        public VocabularyKey Virkningstart { get; set; }
        public VocabularyKey Virkningslut { get; set; }
        public VocabularyKey Vejnavn { get; set; }
        public VocabularyKey Supplerendebynavn { get; set; }
        public VocabularyKey Status { get; set; }
        public VocabularyKey Postnrnavn { get; set; }
        public VocabularyKey Postnr { get; set; }
        public VocabularyKey Id { get; set; }
        public VocabularyKey Husnr { get; set; }
        public VocabularyKey Etage { get; set; }
        public VocabularyKey Dør { get; set; }
        public VocabularyKey Adresseringsvejnavn { get; set; }



    }
}
