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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms
{
    /// <inheritdoc />
    /// <summary>
    /// Component Locale collection
    /// </summary>
    [DataContract]
    public class ComponentLocaleCollection : List<ComponentLocale>
    {
        #region Public Methods
        /// <summary>
        /// Get ComponentLocale from a Market and a Culture
        /// </summary>
        /// <param name="marketKey">Market Key</param>
        /// <param name="cultureKey">Culture Key</param>
        /// <returns>Component Locale instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentLocale Get(string marketKey, string cultureKey)
        {
            if (string.IsNullOrWhiteSpace(marketKey))
                marketKey = "*";
            if (string.IsNullOrWhiteSpace(cultureKey))
                cultureKey = "*";

            return this.FindFirstOf(
                    item => item.MarketKey == marketKey && item.CultureKey == cultureKey,
                    item => item.MarketKey == "*" && item.CultureKey == cultureKey,
                    item => item.MarketKey == marketKey && item.CultureKey == "*",
                    item => item.MarketKey == "*" && item.CultureKey == "*"
                );
        }
        
        /// <summary>
        /// Get Variables from a Merket and a Culture
        /// </summary>
        /// <param name="marketKey">Market Key</param>
        /// <param name="cultureKey">Culture Key</param>
        /// <returns>Variables instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Variables GetVars(string marketKey, string cultureKey)
            => Get(marketKey, cultureKey)?.Vars;
        
        /// <summary>
        /// Remove a ComponentLocale from a Market and a Culture
        /// </summary>
        /// <param name="marketKey">Market Key</param>
        /// <param name="cultureKey">Culture Key</param>
        /// <returns>Removed ComponentLocale instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentLocale Remove(string marketKey, string cultureKey)
        {
            var value = Get(marketKey, cultureKey);
            if (value != null)
                Remove(value);
            return value;
        }
        
        /// <summary>
        /// Removes all ComponentLocales except one defined for a Market and a Culture
        /// </summary>
        /// <param name="marketKey">Market Key</param>
        /// <param name="cultureKey">Culture Key</param>
        /// <returns>The only ComponentLocale instance in the collection</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentLocale RemoveAllBut(string marketKey, string cultureKey)
        {
            var value = Get(marketKey, cultureKey);
            Clear();
            if (value != null)
                Add(value);
            return value;
        }
        #endregion
    }
}
