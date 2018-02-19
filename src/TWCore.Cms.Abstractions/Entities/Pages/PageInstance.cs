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
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Entities.Pages
{
    /// <summary>
    /// Page instance
    /// </summary>
    [DataContract]
    public class PageInstance
    {
        /// <summary>
        /// Market key
        /// </summary>
        [DataMember]
        public string MarketKey { get; set; } = "*";
        /// <summary>
        /// Culture key
        /// </summary>
        [DataMember]
        public string CultureKey { get; set; } = "*";
        /// <summary>
        /// Pages group key
        /// </summary>
        [DataMember]
        public string PagesGroupKey { get; set; } = "*";
        /// <summary>
        /// Site key
        /// </summary>
        [DataMember]
        public string SiteKey { get; set; } = "*";
        /// <summary>
        /// Page routes
        /// </summary>
        [DataMember]
        public List<string> Routes { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Keywords
        /// </summary>
        [DataMember]
        public string Keywords { get; set; }
        /// <summary>
        /// Canonical Url
        /// </summary>
        [DataMember]
        public string CanonicalUrl { get; set; }
        /// <summary>
        /// Publish status
        /// </summary>
        [DataMember]
        public PublishStatus PublishStatus { get; set; }
        /// <summary>
        /// Forward mode
        /// </summary>
        [DataMember]
        public RedirectCode ForwardMode { get; set; }
        /// <summary>
        /// Forward url
        /// </summary>
        [DataMember]
        public string ForwardUrl { get; set; }
        /// <summary>
        /// Variables
        /// </summary>
        [DataMember]
        public Variables Vars { get; set; } = new Variables();
        /// <summary>
        /// Parent component instance
        /// </summary>
        [DataMember]
        public ComponentInstance ParentComponent { get; set; }
    }
}
