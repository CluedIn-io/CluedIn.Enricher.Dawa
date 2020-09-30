using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.Dawa.Vocabularies
{
    /// <summary>A permission identifier vocabulary.</summary>
    /// <seealso cref="T:CluedIn.Core.Data.Vocabularies.SimpleVocabulary"/>
    public abstract class DawaVocabulary : SimpleVocabulary
    {
        protected DawaVocabulary()
        {
            VocabularyName = "Dawa";
            KeyPrefix      = "dawa";
            KeySeparator   = ".";
            Grouping       = Core.Data.EntityType.Location.Address;

            Dawa = Add(new VocabularyKey("dawa", VocabularyKeyVisibility.Hidden));
        }
        
        public VocabularyKey Dawa { get; set; }
    }
}
