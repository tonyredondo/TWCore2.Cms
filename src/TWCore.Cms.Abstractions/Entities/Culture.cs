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
using System.Runtime.Serialization;
using TWCore.Cms.Entities.Common;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// Culture definition
    /// </summary>
    [DataContract]
    public class Culture : AbstractNamedEntity
    {
        /// <summary>
        /// Culture iso tag
        /// </summary>
        [DataMember]
        public string IsoTag { get; set; }
        /// <summary>
        /// First day of the week
        /// </summary>
        [DataMember]
        public DayOfWeek FirstDayOfWeek { get; set; }
        /// <summary>
        /// Short date format
        /// </summary>
        [DataMember]
        public string ShortDateFormat { get; set; }
        /// <summary>
        /// Long date format
        /// </summary>
        [DataMember]
        public string LongDateFormat { get; set; }
        /// <summary>
        /// Short time format
        /// </summary>
        [DataMember]
        public string ShortTimeFormat { get; set; }
        /// <summary>
        /// Long time format
        /// </summary>
        [DataMember]
        public string LongTimeFormat { get; set; }
        /// <summary>
        /// Culture variables
        /// </summary>
        [DataMember]
        public Variables Vars { get; set; } = new Variables();
    }
}
