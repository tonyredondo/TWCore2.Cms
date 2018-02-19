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

using System.Runtime.Serialization;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Cms Page Request
    /// </summary>
    [DataContract]
    public class CmsPageRequest
    {
        /// <summary>
        /// Cms url match
        /// </summary>
        [DataMember]
        public CmsUrlMatch Route { get; set; }
        /// <summary>
        /// Cms page
        /// </summary>
        [DataMember]
        public CmsPage Page { get; set; }
    }
}