using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Vocabularies;
using CluedIn.ExternalSearch.Providers.Dawa;
using CluedIn.Testing.Base.Dummy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace CluedIn.Enricher.Dawa.Integration.Tests
{
    public class DawaTests
    {

        private readonly ITestOutputHelper _outputHelper;

        public DawaTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Theory]
        [InlineData("Titangade 11, 2200 København", "Titangade", "11", "2200", "København N")]
        [InlineData("Nørre Allé 75, 2100 København Ø", "Nørre Allé", "75", "2100", "København Ø")]
        [InlineData("Universitetsparken 1 København Ø", "Universitetsparken", "1", "2100", "København Ø")]
        public void TestClueProductionLocation(string address, string expectedStreetName, string expectedHouseNumber, string expectedZipcode, string expectedZipcodeName)
        {
            // Arrange
            var properties = new EntityMetadataPart();
            properties.Properties.Add(Vocabularies.CluedInOrganization.Address, address);
            IEntityMetadata entityMetadata = new EntityMetadataPart()
            {
                EntityType = EntityType.Location,
                Properties = properties.Properties,
            };

            var externalSearchProvider = new DawaExternalSearchProvider();

            var container = new WindsorContainer();
            container.Register(Component.For(typeof(ILogger<>)).ImplementedBy(typeof(NullLogger<>)).LifestyleSingleton());

            var logger = Mock.Of<ILogger>();
            var applicationContext = new ApplicationContext(container);
            var context = new ExecutionContext(applicationContext, new DummyOrganization(applicationContext), logger);

            var queryParametersDict = new Dictionary<string, HashSet<string>>();
            foreach (var property in entityMetadata.Properties)
            {
                queryParametersDict.Add(property.Key, new HashSet<string> { property.Value });
            }
            var queryParameters = new DummyExternalSearchQueryParameters(queryParametersDict);
            var request = new DummyExternalSearchRequest
            {
                EntityMetaData = entityMetadata,
                QueryParameters = queryParameters
            };

            // Act
            var queries = externalSearchProvider.BuildQueries(context, request);
            var clues = new List<Clue>();
            foreach (var query in queries)
            {
                var results = externalSearchProvider.ExecuteSearch(context, query);
                foreach (var result in results)
                {
                    var resultClues = externalSearchProvider.BuildClues(context, query, result, request);
                    foreach (var clue in resultClues)
                    {
                        clues.Add(clue);
                    }
                }
            }

            // Assert
            Assert.True(clues.Count > 0);
            foreach (var clue in clues)
            {
                clue.Data.EntityData.Properties.TryGetValue("dawa.organization.StreetName", out var streetName);
                Assert.Equal(expectedStreetName, streetName);

                clue.Data.EntityData.Properties.TryGetValue("dawa.organization.HouseNumber", out var houseNumber);
                Assert.Equal(expectedHouseNumber, houseNumber);

                clue.Data.EntityData.Properties.TryGetValue("dawa.organization.Zipcode", out var zipcode);
                Assert.Equal(expectedZipcode, zipcode);

                clue.Data.EntityData.Properties.TryGetValue("dawa.organization.Zipcodename", out var zipcodeName);
                Assert.Equal(expectedZipcodeName, zipcodeName);
            }

            context.Dispose();
        }

    }
}
