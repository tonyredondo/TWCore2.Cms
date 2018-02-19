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
using System.Runtime.Serialization;
using TWCore.Cms.Entities.Common;
using TWCore.Cms.Entities.Pages;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable CollectionNeverUpdated.Global

namespace TWCore.Cms.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Page definition
    /// </summary>
    [DataContract]
    public class Page : AbstractNamedVersionedEntity
    {
        /// <summary>
        /// Behavior pipeline assembly types
        /// </summary>
        [DataMember]
        public List<string> BehaviorPipelineTypes { get; set; }
        /// <summary>
        /// Date from value
        /// </summary>
        [DataMember]
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Date to value
        /// </summary>
        [DataMember]
        public DateTime DateTo { get; set; } = DateTime.MaxValue;
        /// <summary>
        /// Robots tag
        /// </summary>
        [DataMember]
        public RobotsTag Robots { get; set; } = RobotsTag.IndexFollow;
        /// <summary>
        /// Response code
        /// </summary>
        [DataMember]
        public int ResponseCode { get; set; } = 200;
        /// <summary>
        /// Include in the sitemap
        /// </summary>
        [DataMember]
        public bool IncludeInSiteMap { get; set; } = false;
        /// <summary>
        /// Page selection mode
        /// </summary>
        [DataMember]
        public PageInstanceSelectionMode SelectionMode { get; set; } = PageInstanceSelectionMode.First;
        /// <summary>
        /// Page instances collection
        /// </summary>
        [DataMember]
        public PageInstanceCollection Instances { get; set; }
    }
}
