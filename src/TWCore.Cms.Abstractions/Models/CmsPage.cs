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

// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Cms page
    /// </summary>
    [DataContract]
    public class CmsPage
    {
        /// <summary>
        /// Market
        /// </summary>
        [DataMember]
        public Market Market { get; set; }
        /// <summary>
        /// Culture
        /// </summary>
        [DataMember]
        public Culture Culture { get; set; }
        /// <summary>
        /// Site
        /// </summary>
        [DataMember]
        public Site Site { get; set; }
        /// <summary>
        /// Pages group
        /// </summary>
        [DataMember]
        public PagesGroup PagesGroup { get; set; }
        /// <summary>
        /// Page
        /// </summary>
        [DataMember]
        public Page Page { get; set; }
        /// <summary>
        /// Url binding collection
        /// </summary>
        [DataMember]
        public CmsUrlBindingCollection Uris { get; set; }
        //

        /// <summary>
        /// Header
        /// </summary>
        [DataMember]
        public CmsComponentInstance Header { get; set; }
        /// <summary>
        /// Footer
        /// </summary>
        [DataMember]
        public CmsComponentInstance Footer { get; set; }
        /// <summary>
        /// Parent component
        /// </summary>
        [DataMember]
        public CmsComponentInstance ParentComponent { get; set; }


        //
        /// <summary>
        /// Styles
        /// </summary>
        [DataMember]
        public Dictionary<string, Stylesheet> Styles { get; set; }
        /// <summary>
        /// Scripts
        /// </summary>
        [DataMember]
        public Dictionary<string, Script> Scripts { get; set; }
        /// <summary>
        /// Current page variables
        /// </summary>
        [DataMember]
        public Variables CurrentPageVariables { get; set; }
        /// <summary>
        /// Messages
        /// </summary>
        [DataMember]
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        /// <summary>
        /// Hash
        /// </summary>
        [DataMember]
        public string Hash { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        [DataMember]
        public string Path { get; set; }
        /// <summary>
        /// View path
        /// </summary>
        [DataMember]
        public string ViewPath { get; set; }


        #region Public Methods
        /// <summary>
        /// Get Cms Page model
        /// </summary>
        /// <returns>CmsPageModel instance</returns>
        public CmsPageModel GetModel()
            => new CmsPageModel(this);
        #endregion
    }
}
