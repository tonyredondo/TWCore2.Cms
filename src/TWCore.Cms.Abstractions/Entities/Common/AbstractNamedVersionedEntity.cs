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

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Entities.Common
{
    /// <inheritdoc />
    /// <summary>
    /// Abstract named versioned entity
    /// </summary>
    [DataContract]
    public abstract class AbstractNamedVersionedEntity : AbstractNamedEntity
    {
        private int _rev;
        private string _keyWithRev;
        private string _oldKey;

        /// <summary>
        /// Key with revision
        /// </summary>
        [DataMember]
        public string KeyWithRev
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!string.IsNullOrEmpty(_keyWithRev) && _oldKey == Key) return _keyWithRev;
                _oldKey = Key;
                _keyWithRev = Key + ":" + Rev;
                return _keyWithRev;
            }
        }
        /// <summary>
        /// Revision
        /// </summary>
        [DataMember]
        public int Rev
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _rev;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _rev = value;
                _keyWithRev = null;
            }
        }
        /// <summary>
        /// Revision comment
        /// </summary>
        [DataMember]
        public string RevComment { get; set; }
    }
}
