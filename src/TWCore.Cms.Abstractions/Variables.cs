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
// ReSharper disable MemberCanBePrivate.Global

namespace TWCore.Cms
{
    /// <summary>
    /// Variables
    /// </summary>
    [DataContract]
    public class Variables
    {
        /// <summary>
        /// Styles
        /// </summary>
        [DataMember]
        public ValueCollection Styles { get; set; } = new ValueCollection();
        /// <summary>
        /// Scripts
        /// </summary>
        [DataMember]
        public ValueCollection Scripts { get; set; } = new ValueCollection();
        /// <summary>
        /// Data
        /// </summary>
        [DataMember]
        public ValueDictionary Data { get; set; } = new ValueDictionary();

        #region Public Methods
        /// <summary>
        /// Adds a Variables items and returns a new Variable instance
        /// </summary>
        /// <param name="item">Variable instance to add</param>
        /// <returns>New Variables instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variables Add(Variables item)
        {
            return new Variables
            {
                Styles = Styles != null ? Styles.Add(item?.Styles) : item?.Styles?.Add(null),
                Scripts = Scripts != null ? Scripts.Add(item?.Scripts) : item?.Scripts?.Add(null),
                Data = Data != null ? Data.Add(item?.Data) : item?.Data?.Add(null),
            };
        }
        #endregion

        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Variables operator +(Variables item1, Variables item2) 
            => item1 != null ? item1.Add(item2) : item2?.Add(null);
        #endregion
    }
}
