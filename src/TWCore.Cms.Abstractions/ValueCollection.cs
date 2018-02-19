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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global

namespace TWCore.Cms
{
    /// <inheritdoc />
    /// <summary>
    /// Value collection
    /// </summary>
    [DataContract]
    public class ValueCollection : List<ValueItem>
    {
        #region .ctor
        /// <inheritdoc />
        /// <summary>
        /// Value collection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueCollection() { }
        /// <inheritdoc />
        /// <summary>
        /// Value collection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueCollection(IEnumerable<ValueItem> col) : base(col) { }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Adds a ValueCollection to this ValueCollection
        /// </summary>
        /// <param name="item">ValueCollection instance</param>
        /// <returns>New Value Collection with the merge of the two collections</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueCollection Add(ValueCollection item)
        {
            var newCollection = new ValueCollection(this.Where(i => i?.Option == ValueOption.Include));
            var count = item?.Count ?? 0;
            for (var i = 0; i < count; i++)
            {
                var value = item?[i];
                if (value == null)
                    continue;
                var existingValue = newCollection.Find(nItem =>
                {
                    if (nItem.Type != value.Type) return false;
                    switch(nItem.Type)
                    {
                        case ValueType.Text:
                            return string.Equals(nItem.SValue, value.SValue, StringComparison.Ordinal);
                        case ValueType.Bool:
                            return nItem.BValue == value.BValue;
                        case ValueType.Integer:
                            return nItem.IValue == value.IValue;
                        case ValueType.Float:
                            if (!nItem.FValue.HasValue && !value.FValue.HasValue) return true;
                            if (!nItem.FValue.HasValue) return false;
                            if (!value.FValue.HasValue) return false;
                            return Math.Abs(nItem.FValue.Value - value.FValue.Value) < 0.00001;
                        case ValueType.Decimal:
                            return nItem.DValue == value.DValue;
                        case ValueType.Unknown:
                            return false;
                        default:
                            return false;
                    }
                });
                if (existingValue == null)
                {
                    newCollection.Add(value);
                }
                else
                {
                    newCollection.Remove(existingValue);
                    if (value.Option == ValueOption.Include)
                        newCollection.Add(value);
                }
            }
            return newCollection;
        }
        #endregion
        
        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ValueCollection operator +(ValueCollection item1, ValueCollection item2) 
            => item1.Add(item2);
        #endregion
    }
}
