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

namespace TWCore.Cms.Entities.Sites
{
    /// <summary>
    /// Tracker definition
    /// </summary>
    [DataContract]
    public class Tracker
    {
        /// <summary>
        /// Tracker key
        /// </summary>
        [DataMember]
        public string TrackerKey { get; set; }
        /// <summary>
        /// Parameters
        /// </summary>
        [DataMember]
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
    }
}
