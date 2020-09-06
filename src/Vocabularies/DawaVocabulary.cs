using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Dawa.Vocabularies
{
    /// <summary>A permission identifier vocabulary.</summary>
    /// <seealso cref="T:CluedIn.Core.Data.Vocabularies.SimpleVocabulary"/>
    public abstract class DawaVocabulary : SimpleVocabulary
    {
        protected DawaVocabulary()
        {
            this.VocabularyName = "Dawa";
            this.KeyPrefix      = "dawa";
            this.KeySeparator   = ".";
            this.Grouping       = CluedIn.Core.Data.EntityType.Location.Address;

            this.Dawa = this.Add(new VocabularyKey("dawa", VocabularyKeyVisibility.Hidden));
        }
        
        public VocabularyKey Dawa { get; set; }
    }
}
