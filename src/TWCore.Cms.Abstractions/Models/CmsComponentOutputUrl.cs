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

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Cms component output url
    /// </summary>
    [DataContract]
    public class CmsComponentOutputUrl
    {
        /// <summary>
        /// Url key
        /// </summary>
        [DataMember]
        public string UrlKey { get; set; }
        /// <summary>
        /// Internal url
        /// </summary>
        [DataMember]
        public bool InternalUrl { get; set; }
        /// <summary>
        /// Required components on destination
        /// </summary>
        [DataMember]
        public List<string> RequiredComponentsKeyOnDestination { get; set; }
        /// <summary>
        /// Page key
        /// </summary>
        [DataMember]
        public string PageKey { get; set; }
        /// <summary>
        /// External url
        /// </summary>
        [DataMember]
        public string ExternalUrl { get; set; }
    }
}
