using CluedIn.Core.Data.Parts;
using CluedIn.ExternalSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace CluedIn.Enricher.Dawa.Integration.Tests
{
    public class DummyExternalSearchRequest : IExternalSearchRequest
    {
        public IEntityMetadata EntityMetaData { get; set; }
        public object CustomQueryInput { get; set; } = null;
        public bool? NoRecursion { get; set; } = null;
        public List<Guid> ProviderIds { get; set; } = null;
        public IExternalSearchQueryParameters QueryParameters { get; set; } = null;
        public List<IExternalSearchQuery> Queries { get; set; } = null;
        public bool IsFinished { get; set; } = false;
        public bool AllQueriesHasExecuted { get; } = false;
    }
}
