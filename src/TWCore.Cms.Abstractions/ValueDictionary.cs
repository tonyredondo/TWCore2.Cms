/*
Copyright 2018 Daniel Adrian Redondo Suarez

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
 */

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms
{
    /// <inheritdoc />
    /// <summary>
    /// Value dictionary
    /// </summary>
    [DataContract]
    public class ValueDictionary : Dictionary<string, ValueItem>
    {
        #region .ctor
        /// <inheritdoc />
        /// <summary>
        /// Value dictionary
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueDictionary() { }
        /// <inheritdoc />
        /// <summary>
        /// Value dictionary
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueDictionary(IDictionary<string, ValueItem> col) : base(col) { }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Adds a ValueDictionary to this ValueDictionary
        /// </summary>
        /// <param name="item">ValueDictionary instance</param>
        /// <returns>New ValueDictionary with the merge of the two dictionaries</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueDictionary Add(ValueDictionary item)
        {
            if (item == null)
            {
                var col = new ValueDictionary(this);
                col.RemoveCollection(i => i.Value?.Option == ValueOption.Exclude);
                return col;
            }

            var newCollection = new ValueDictionary(this.Where(i => i.Value?.Option == ValueOption.Include).ToDictionary(k => k.Key, k => k.Value));
            if (!item.Any())
                return newCollection;
            
            foreach (var key in item.Keys)
            {
                var value = item[key];
                if (value == null) continue;
                
                if (newCollection.TryGetValue(key, out var existingValue))
                {
                    newCollection.Remove(key);
                    if (value.Option == ValueOption.Include)
                        newCollection.Add(key, value);
                }
                else
                    newCollection.Add(key, value);
            }
            return newCollection;
        }

        /// <summary>
        /// Gets the ValueDictionary instance in its indexed mode
        /// </summary>
        /// <returns>ValueDictionaryIndexed instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueDictionaryIndexed GetIndexer() 
            => new ValueDictionaryIndexed(this);
        #endregion

        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueDictionary operator +(ValueDictionary item1, ValueDictionary item2)
            => item1.Add(item2);
        #endregion

        #region Nested Types
        /// <summary>
        /// ValueDictionary Indexed
        /// </summary>
        [DataContract]
        public class ValueDictionaryIndexed
        {
            /// <summary>
            /// ValueDictionary instance
            /// </summary>
            public ValueDictionary Items { get; set; }

            #region .ctor
            /// <summary>
            /// ValueDictionary Indexed
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ValueDictionaryIndexed() { }
            /// <summary>
            /// ValueDictionary Indexed
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ValueDictionaryIndexed(ValueDictionary items) 
                => Items = items;
            #endregion
            
            #region Public Methods
            /// <summary>
            /// Item Indexer
            /// </summary>
            /// <param name="key">Key</param>
            public object this[string key]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => Items[key].Value;
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => Items[key] = new ValueItem(value);
            }
            #endregion
        }
        #endregion
    }
}
