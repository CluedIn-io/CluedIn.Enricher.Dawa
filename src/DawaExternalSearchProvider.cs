
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
using CluedIn.Core.Messages.WebApp;

namespace CluedIn.ExternalSearch.Providers.Dawa
{
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class DawaExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata
    {
        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        public static Guid DawaProviderId = Guid.Parse("886ebbe3-d9ee-401f-b8e8-93ee215f451e");

        public DawaExternalSearchProvider()
            : base(DawaProviderId, EntityType.Organization)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        /// <inheritdoc/>
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            if (!Accepts(request.EntityMetaData.EntityType))
                yield break;

            // Query Input
            var entityType = request.EntityMetaData.EntityType;
            var address = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Address, new HashSet<string>());

            if (address != null)
            {
                var values = address.GetOrganizationNameVariants().Select(NameNormalization.Normalize).ToHashSet();

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
            var response = client.Execute(request);
            var data = JsonConvert.DeserializeObject<AddressResponse>(response.Content);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var allowedCategories = new string[] { "A", "B" };
                if (data != null && allowedCategories.Contains(data.Category))
                {
                    foreach (var result in data.Results)
                    {
                        yield return new ExternalSearchQueryResult<Result>(query, result);
                    }
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
            var resultItem = result.As<Result>();

            string id;
            if (request.QueryParameters.ContainsKey(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesCVR))
            {
                id = request.QueryParameters.GetValue(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesCVR).FirstOrDefault();
            }
            else
            {
                id = resultItem.Data.ActualAddress.AccessAddressId;
            }

            var code = new EntityCode(EntityType.Organization, CodeOrigin.CluedIn, id);

            var clue = new Clue(code, context.Organization);

            clue.Data.OriginProviderDefinitionId = Id;

            PopulateMetadata(clue.Data.EntityData, resultItem, request);

            yield return clue;
        }

        /// <inheritdoc/>
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<Result>();

            if (resultItem == null)
                return null;

            return CreateMetadata(resultItem, request);
        }

        /// <inheritdoc/>
        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return null;
        }

        /// <summary>Creates the metadata.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<Result> resultItem, IExternalSearchRequest request)
        {
            if (resultItem == null)
                throw new ArgumentNullException(nameof(resultItem));

            var metadata = new EntityMetadataPart();

            PopulateMetadata(metadata, resultItem, request);

            return metadata;
        }

        /// <summary>Gets the origin entity code.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The origin entity code.</returns>
        private EntityCode GetOriginEntityCode(IExternalSearchQueryResult<Result> resultItem)
        {
            if (resultItem == null)
                throw new ArgumentNullException(nameof(resultItem));

            return new EntityCode(EntityType.Organization, GetCodeOrigin(), resultItem.Data.Address.AccessAddressId);
        }

        /// <summary>Gets the code origin.</summary>
        /// <returns>The code origin</returns>
        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn;
        }

        /// <summary>Populates the metadata.</summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<Result> resultItem, IExternalSearchRequest request)
        {
            if (resultItem == null)
                throw new ArgumentNullException(nameof(resultItem));

            var code = GetOriginEntityCode(resultItem);
            var data = resultItem.Data;

            metadata.Name = request.QueryParameters.GetValue(Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName, new HashSet<string>()).FirstOrDefault();
            metadata.EntityType = EntityType.Organization;
            metadata.CreatedDate = resultItem.CreatedDate;

            metadata.OriginEntityCode = code;
            metadata.Codes.Add(code);

            metadata.Properties[DamaVocabularies.Organization.AdressingStreetName] = data.ActualAddress.AdressingStreetName.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Door] = data.ActualAddress.Door.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Floor] = data.ActualAddress.Floor.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.HouseNumber] = data.ActualAddress.HouseNumber.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Id] = data.ActualAddress.Id.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Zipcode] = data.ActualAddress.Zipcode.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.ZipcodeName] = data.ActualAddress.Zipcodename.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.Status] = data.ActualAddress.Status.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.AdditionalCityName] = data.ActualAddress.AdditionalCityName.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.StreetName] = data.ActualAddress.StreetName.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.EffectiveEnd] = data.ActualAddress.EffectiveEnd.PrintIfAvailable();
            metadata.Properties[DamaVocabularies.Organization.EffectiveStart] = data.ActualAddress.EffectiveStart.PrintIfAvailable();

            metadata.Uri = new Uri(data.ActualAddress.Href);
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
