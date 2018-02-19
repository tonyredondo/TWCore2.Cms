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
using TWCore.Cms.Entities.Pages;
using TWCore.Cms.Entities.Sites;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TWCore.Cms.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Site definition
    /// </summary>
    [DataContract]
    public class Site : AbstractNamedVersionedEntity
    {
        /// <summary>
        /// Url Bindings
        /// </summary>
        [DataMember]
        public List<UrlBinding> UrlBindings { get; set; } = new List<UrlBinding>();
        /// <summary>
        /// Market Key
        /// </summary>
        [DataMember]
        public string MarketKey { get; set; }
        /// <summary>
        /// Culture Key
        /// </summary>
        [DataMember]
        public string CultureKey { get; set; }
        /// <summary>
        /// Pages Groups
        /// </summary>
        [DataMember]
        public List<string> PagesGroups { get; set; } = new List<string>();
        /// <summary>
        /// Header component instance
        /// </summary>
        [DataMember]
        public ComponentInstance Header { get; set; }
        /// <summary>
        /// Footer component instance
        /// </summary>
        [DataMember]
        public ComponentInstance Footer { get; set; }
        /// <summary>
        /// Tracker list
        /// </summary>
        [DataMember]
        public List<Tracker> Trackers { get; set; }
        /// <summary>
        /// Site Variables
        /// </summary>
        [DataMember]
        public Variables Vars { get; set; } = new Variables();
        /// <summary>
        /// Gets or Sets if the site is published
        /// </summary>
        [DataMember]
        public bool Published { get; set; }
    }
}
