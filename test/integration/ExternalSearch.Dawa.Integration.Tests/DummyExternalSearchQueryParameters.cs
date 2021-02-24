using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Vocabularies;
using CluedIn.ExternalSearch;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CluedIn.Enricher.Dawa.Integration.Tests
{
    public class DummyExternalSearchQueryParameters : IExternalSearchQueryParameters
    {
        private IDictionary<string, HashSet<string>> _values;

        public DummyExternalSearchQueryParameters(IDictionary<string, HashSet<string>> values)
        {
            _values = values;
        }

        public HashSet<string> Get(VocabularyKey key, HashSet<string> defaultValue)
        {
            if (_values.ContainsKey(key))
                return _values[key];
            else
                return defaultValue;
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out HashSet<string> value)
        {
            if (_values.ContainsKey(key))
            {
                value = _values[key];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool ContainsKey(Enum key)
        {
            return _values.ContainsKey(key.ToString());
        }

        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public HashSet<string> this[Enum key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public HashSet<string> this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsEmptyQuery => throw new NotImplementedException();

        public bool IsModified { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<HashSet<string>> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(string key, string value)
        {
            throw new NotImplementedException();
        }

        public void Add(ExternalSearchQueryParameter key, string value)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, IEnumerable<string> values)
        {
            throw new NotImplementedException();
        }

        public void Add(ExternalSearchQueryParameter key, IEnumerable<string> values)
        {
            throw new NotImplementedException();
        }

        public void Add(string key, HashSet<string> value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, HashSet<string>> item)
        {
            throw new NotImplementedException();
        }

        public void AddKeyValuePairs(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            throw new NotImplementedException();
        }

        public void AddKeyValuePairs(IEntityMetadata entityMetadata)
        {
            throw new NotImplementedException();
        }

        public string BuildQueryKey()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, HashSet<string>> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, HashSet<string>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, HashSet<string>>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, HashSet<string>> item)
        {
            throw new NotImplementedException();
        }

        public void ResetStatus()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
