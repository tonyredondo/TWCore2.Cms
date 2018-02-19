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
using System.Runtime.Serialization;
using TWCore.Cms.Entities.Common;
using TWCore.Cms.Entities.Components;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace TWCore.Cms.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Component Definition
    /// </summary>
    [DataContract]
    public class Component : AbstractNamedVersionedEntity
    {
        /// <summary>
        /// Placeholder Type
        /// </summary>
        [DataMember]
        public ComponentPlaceholderType PlaceholderType { get; set; }
        /// <summary>
        /// Component Class Type
        /// </summary>
        [DataMember]
        public string ComponentClassType { get; set; }
        /// <summary>
        /// Output Urls
        /// </summary>
        [DataMember]
        public List<ComponentOutputUrl> OutputUrls { get; set; }
        /// <summary>
        /// Component Locales
        /// </summary>
        [DataMember]
        public ComponentLocaleCollection Locales { get; set; } = new ComponentLocaleCollection();
        /// <summary>
        /// Get or Sets if the components can pre render the vars
        /// </summary>
        [DataMember]
        public bool PreRenderVars { get; set; } = false;
        /// <summary>
        /// Cache timeout in minutes
        /// </summary>
        [DataMember]
        public int CacheTimeoutInMinutes { get; set; } = 0;
    }
}
