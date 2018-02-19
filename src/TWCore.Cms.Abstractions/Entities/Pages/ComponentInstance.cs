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
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable ConvertToAutoPropertyWithPrivateSetter

namespace TWCore.Cms.Entities.Pages
{
    /// <summary>
    /// Component Instance
    /// </summary>
    [DataContract]
    public class ComponentInstance
    {
        private string _componentKeyAndRev;
        private string _componentKey;
        private int _componentRev;

        /// <summary>
        /// Component Key and Revision
        /// </summary>
        [DataMember]
        public string ComponentKeyAndRev => _componentKeyAndRev;

        /// <summary>
        /// Component Key
        /// </summary>
        [DataMember]
        public string ComponentKey
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _componentKey;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _componentKey = value;
                _componentKeyAndRev = _componentKey + ":" + _componentRev;
            }
        }
        /// <summary>
        /// Component Revision
        /// </summary>
        [DataMember]
        public int ComponentRev
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _componentRev;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _componentRev = value;
                _componentKeyAndRev = _componentKey + ":" + _componentRev;
            }
        }
        /// <summary>
        /// Placeholder zone
        /// </summary>
        [DataMember]
        public string PlaceholderZone { get; set; }
        /// <summary>
        /// Layout order
        /// </summary>
        [DataMember]
        public int LayoutOrder { get; set; }
        /// <summary>
        /// Date from
        /// </summary>
        [DataMember]
        public DateTime DateFrom { get; set; }
        /// <summary>
        /// Date to
        /// </summary>
        [DataMember]
        public DateTime DateTo { get; set; }
        /// <summary>
        /// Variables
        /// </summary>
        [DataMember]
        public Variables Vars { get; set; }
        /// <summary>
        /// Output urls
        /// </summary>
        [DataMember]
        public List<ComponentOutputUrlInstance> OutputUrls { get; set; }
        /// <summary>
        /// Component childrens
        /// </summary>
        [DataMember]
        public List<ComponentInstance> Children { get; set; }
    }
}
