using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Dawa.Vocabularies
{
    public class DawaOrganizationVocabulary : DawaVocabulary
    {
        public DawaOrganizationVocabulary()
        {
            VocabularyName = "Dawa Organization";
            KeyPrefix = "dawa.organization";
            KeySeparator = ".";
            Grouping = EntityType.Organization;

            AddGroup("Address Data", group =>
                {
                    AdressingStreetName = group.Add(
                        new VocabularyKey("AdressingStreetName", VocabularyKeyDataType.Text)
                        .WithDescription("Adresseringsvejnavn")
                    );

                    AdditionalCityName = group.Add(
                        new VocabularyKey("AdditionalCityName", VocabularyKeyDataType.Text)
                        .WithDescription("Supplerende bynavn")
                    );

                    Door = group.Add(
                        new VocabularyKey("Door", VocabularyKeyDataType.Text)
                        .WithDescription("Dør")
                    );

                    EffectiveStart = group.Add(
                        new VocabularyKey("EffectiveStart", VocabularyKeyDataType.DateTime)
                        .WithDescription("Virkning slut")
                    );

                    EffectiveEnd = group.Add(
                        new VocabularyKey("EffectiveEnd", VocabularyKeyDataType.DateTime)
                        .WithDescription("Virkning start")
                    );

                    Floor = group.Add(
                        new VocabularyKey("Floor", VocabularyKeyDataType.Text)
                        .WithDescription("Etage")
                    );

                    HouseNumber = group.Add(
                        new VocabularyKey("HouseNumber", VocabularyKeyDataType.Text)
                        .WithDescription("Husnr.")
                    );

                    Id = group.Add(
                        new VocabularyKey("Id", VocabularyKeyDataType.Identifier)
                        .WithDescription("Id")
                    );

                    Status = group.Add(
                        new VocabularyKey("Status", VocabularyKeyDataType.Number)
                        .WithDescription("Status")
                    );

                    StreetName = group.Add(
                        new VocabularyKey("StreetName", VocabularyKeyDataType.Text)
                        .WithDescription("Vejnavn")
                    );

                    Zipcode = group.Add(
                        new VocabularyKey("Zipcode", VocabularyKeyDataType.Text)
                        .WithDescription("Postnummer")
                    );

                    ZipcodeName = group.Add(
                        new VocabularyKey("Zipcodename", VocabularyKeyDataType.Text)
                        .WithDescription("Postnummer navn")
                    );

                });

            AddMapping(AdressingStreetName, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressStreetName);
            AddMapping(StreetName, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressStreetName);
            AddMapping(Floor, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressFloorCode);
            AddMapping(HouseNumber, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressStreetNumber);
            AddMapping(Zipcode, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressZipCode);
            AddMapping(ZipcodeName, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressPostalArea);
            AddMapping(ZipcodeName, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.AddressCity);
        }

        public VocabularyKey AdditionalCityName { get; set; }
        public VocabularyKey AdressingStreetName { get; set; }
        public VocabularyKey Door { get; set; }
        public VocabularyKey EffectiveEnd { get; set; }
        public VocabularyKey EffectiveStart { get; set; }
        public VocabularyKey Floor { get; set; }
        public VocabularyKey HouseNumber { get; set; }
        public VocabularyKey Id { get; set; }
        public VocabularyKey Status { get; set; }
        public VocabularyKey StreetName { get; set; }
        public VocabularyKey Zipcode { get; set; }
        public VocabularyKey ZipcodeName { get; set; }

    }
}
