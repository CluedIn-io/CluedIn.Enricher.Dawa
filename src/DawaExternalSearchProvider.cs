
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Newtonsoft.Json;
using RestSharp;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.ExternalSearch.Filters;
using CluedIn.ExternalSearch.Providers.Dawa.Models;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Providers.Dawa.Vocabularies;
using EntityType = CluedIn.Core.Data.EntityType;

namespace CluedIn.ExternalSearch.Providers.Dawa
{
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class DawaExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata
    {
        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        public DawaExternalSearchProvider()
            : this(true)
        {
            var nameBasedTokenProvider = new NameBasedTokenProvider("Dawa");

            if (nameBasedTokenProvider.ApiToken != null)
                this.TokenProvider = new RoundRobinTokenProvider(nameBasedTokenProvider.ApiToken.Split(',', ';'));
        }

        public DawaExternalSearchProvider(IList<string> tokens)
            : this(true)
        {
            this.TokenProvider = new RoundRobinTokenProvider(tokens);
        }

        public DawaExternalSearchProvider([NotNull] IExternalSearchTokenProvider tokenProvider)
            : this(true)
        {
            this.TokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        private DawaExternalSearchProvider(bool tokenProviderIsRequired)
            : base(Constants.ExternalSearchProviders.PermId, EntityType.Organization)
        {
            this.TokenProviderIsRequired = tokenProviderIsRequired;
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        /// <inheritdoc/>
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            if (!this.Accepts(request.EntityMetaData.EntityType))
                yield break;


            // Query Input
            var entityType = request.EntityMetaData.EntityType;
            var address = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Address, new HashSet<string>());

            if (address != null)
            {
                var values = address.GetOrganizationNameVariants()
                                             .Select(NameNormalization.Normalize)
                                             .ToHashSet();

                foreach (var value in values)
                {


                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Name, value);
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query)
        {
            var address = query.QueryParameters[ExternalSearchQueryParameter.Name].FirstOrDefault();

            var client = new RestClient("https://dawa.aws.dk/datavask/");
            var request = new RestRequest("adresser", Method.GET);
            request.AddParameter("betegnelse", address);
            var response = client.Execute<AddressResponse>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data != null)
                {
                    yield return new ExternalSearchQueryResult<List<Resultater>>(query, response.Data.Resultater);
                }
            }
            else if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                yield break;
            else if (response.ErrorException != null)
                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
            else
                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode);
        }


        /// <inheritdoc/>
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<Resultater>();



            var code = new EntityCode(EntityType.Person, CodeOrigin.CluedIn.CreateSpecific("dawa"), resultItem.Data.Adresse.Adgangsadresseid);

            var clue = new Clue(code, context.Organization);

            clue.Data.OriginProviderDefinitionId = this.Id;

            this.PopulateMetadata(clue.Data.EntityData, resultItem);

            yield return clue;


        }

        /// <inheritdoc/>
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<Resultater>();

            if (resultItem == null)
                return null;

            return this.CreateMetadata(resultItem);
        }

        /// <inheritdoc/>
        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return null;
        }

        /// <summary>Creates the metadata.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<Resultater> resultItem)
        {
            if (resultItem == null)
                throw new ArgumentNullException(nameof(resultItem));

            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(metadata, resultItem);

            return metadata;
        }

        /// <summary>Gets the origin entity code.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The origin entity code.</returns>
        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<Resultater> resultItem)
        {
            if (resultItem == null)
                throw new ArgumentNullException(nameof(resultItem));

            return new EntityCode(EntityType.Organization, this.GetCodeOrigin(), resultItem.Data.Adresse.Adgangsadresseid);
        }

        /// <summary>Gets the code origin.</summary>
        /// <returns>The code origin</returns>
        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("dawa");
        }

        /// <summary>Populates the metadata.</summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<Resultater> resultItem)
        {
            if (resultItem == null)
                throw new ArgumentNullException(nameof(resultItem));

            var code = this.GetOriginEntityCode(resultItem);
            var data = resultItem.Data;

            metadata.EntityType = EntityType.Organization;
            metadata.CreatedDate = resultItem.CreatedDate;

            metadata.OriginEntityCode = code;
            metadata.Codes.Add(code);

            metadata.Properties[DamaVocabularies.Organization.Adresseringsvejnavn] = data.Aktueladresse.Adresseringsvejnavn.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Dør] = data.Aktueladresse.Dør.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Etage] = data.Aktueladresse.Etage.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Husnr] = data.Aktueladresse.Husnr.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Id] = data.Aktueladresse.Id.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Postnr] = data.Aktueladresse.Postnr.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Postnrnavn] = data.Aktueladresse.Postnrnavn.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Status] = data.Aktueladresse.Status.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Supplerendebynavn] = data.Aktueladresse.Supplerendebynavn.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Vejnavn] = data.Aktueladresse.Vejnavn.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Virkningslut] = data.Aktueladresse.Virkningslut.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Virkningstart] = data.Aktueladresse.Virkningstart.PrintIfAvailable();

            metadata.Uri = new Uri(data.Aktueladresse.Href);

        }



        public string Icon { get; } = "Resources.dawa.jpg";
        public string Domain { get; } = "https://dawa.aws.dk/";
        public string About { get; } = "Danmarks Adressers Web API (DAWA) exhibits data and functionality regarding Denmark's addresses. DAWA can be used to establish address data and functionality in IT systems. The target audience for this website is developers who want to implement address data and functionality in their IT systems.";
        public AuthMethods AuthMethods { get; } = null;
        public IEnumerable<Control> Properties { get; } = null;
        public Guide Guide { get; } = null;
        public IntegrationType Type { get; } = IntegrationType.Cloud;
    }
}
