﻿/*
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
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBeProtected.Global

namespace TWCore.Cms
{
    /// <summary>
    /// Localized Variables per Market and Culture
    /// </summary>
    [DataContract]
    public class LocalizedVariables
    {
        /// <summary>
        /// Market Key
        /// </summary>
        [DataMember]
        public string MarketKey { get; set; } = "*";
        /// <summary>
        /// Culture Key
        /// </summary>
        [DataMember]
        public string CultureKey { get; set; } = "*";
        /// <summary>
        /// Variables
        /// </summary>
        [DataMember]
        public Variables Vars { get; set; }

        #region .ctor
        /// <summary>
        /// Localized Variables per Market and Culture
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LocalizedVariables() { }
        /// <summary>
        /// Localized Variables per Market and Culture
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LocalizedVariables(string marketKey, string cultureKey, Variables vars)
        {
            MarketKey = marketKey;
            CultureKey = cultureKey;
            Vars = vars;
        }
        /// <summary>
        /// Localized Variables per Market and Culture
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LocalizedVariables(Variables vars)
        {
            MarketKey = "*";
            CultureKey = "*";
            Vars = vars;
        }
        #endregion
    }
}
