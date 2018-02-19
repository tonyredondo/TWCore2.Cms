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

using System.Runtime.Serialization;
using TWCore.Cms.Entities.Common;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Script definition
    /// </summary>
    [DataContract]
    public class Script : AbstractNamedEntity
    {
        /// <summary>
        /// Collection type
        /// </summary>
        [DataMember]
        public CollectionType CollectionType { get; set; }
        /// <summary>
        /// Script type
        /// </summary>
        [DataMember]
        public ScriptType Type { get; set; }
        /// <summary>
        /// Script Load type
        /// </summary>
        [DataMember]
        public ScriptLoad LoadType { get; set; }
        /// <summary>
        /// Source or Value
        /// </summary>
        [DataMember]
        public string Source { get; set; }
    }
}
