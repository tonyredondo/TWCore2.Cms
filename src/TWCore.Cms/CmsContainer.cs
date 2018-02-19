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
using TWCore.Cms.Entities;

namespace TWCore.Cms
{
    /// <summary>
    /// Cms Container
    /// </summary>
    [DataContract]
    public class CmsContainer
    {
        /// <summary>
        /// Markets
        /// </summary>
        [DataMember]
        public List<Market> Markets { get; set; } = new List<Market>();
        /// <summary>
        /// Cultures
        /// </summary>
        [DataMember]
        public List<Culture> Cultures { get; set; } = new List<Culture>();
        /// <summary>
        /// Stylesheets
        /// </summary>
        [DataMember]
        public List<Stylesheet> Stylesheets { get; set; } = new List<Stylesheet>();
        /// <summary>
        /// Scripts
        /// </summary>
        [DataMember]
        public List<Script> Scripts { get; set; } = new List<Script>();
        /// <summary>
        /// Components
        /// </summary>
        [DataMember]
        public List<Component> Components { get; set; } = new List<Component>();
        /// <summary>
        /// Sites
        /// </summary>
        [DataMember]
        public List<Site> Sites { get; set; } = new List<Site>();
        /// <summary>
        /// Pages Groups
        /// </summary>
        [DataMember]
        public List<PagesGroup> PagesGroups { get; set; } = new List<PagesGroup>();
        /// <summary>
        /// Pages
        /// </summary>
        [DataMember]
        public List<Page> Pages { get; set; } = new List<Page>();
        /// <summary>
        /// Users
        /// </summary>
        [DataMember]
        public List<User> Users { get; set; } = new List<User>();
    }
}