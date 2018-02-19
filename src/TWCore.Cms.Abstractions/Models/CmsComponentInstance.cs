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
using System.Linq;
using System.Runtime.Serialization;
using TWCore.Cms.Entities;
using TWCore.Cms.Entities.Pages;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Cms Component instance
    /// </summary>
    [DataContract]
    public class CmsComponentInstance
    {
        #region Properties
        /// <summary>
        /// Component Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// Component Key
        /// </summary>
        [DataMember]
        public string Key { get; set; }
        /// <summary>
        /// Update date
        /// </summary>
        [DataMember]
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// Gets or Sets Enabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        [DataMember]
        public int Rev { get; set; }
        /// <summary>
        /// Revision comment
        /// </summary>
        [DataMember]
        public string RevComment { get; set; }
        /// <summary>
        /// Placeholder type
        /// </summary>
        [DataMember]
        public ComponentPlaceholderType PlaceholderType { get; set; }
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
        /// Component class type
        /// </summary>
        [DataMember]
        public string ComponentClassType { get; set; }
        /// <summary>
        /// View content type
        /// </summary>
        [DataMember]
        public ComponentContentType ViewContentType { get; set; }
        /// <summary>
        /// View content
        /// </summary>
        [DataMember]
        public string ViewContent { get; set; }
        /// <summary>
        /// Editor content type
        /// </summary>
        [DataMember]
        public ComponentContentType EditorContentType { get; set; }
        /// <summary>
        /// Editor content
        /// </summary>
        [DataMember]
        public string EditorContent { get; set; }
        /// <summary>
        /// Output urls
        /// </summary>
        [DataMember]
        public List<CmsComponentOutputUrl> OutputUrls { get; set; }
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
        /// Component childrens
        /// </summary>
        [DataMember]
        public List<CmsComponentInstance> Children { get; set; }
        /// <summary>
        /// Pre render the variables
        /// </summary>
        [DataMember]
        public bool PreRenderVars { get; set; }
        /// <summary>
        /// Cache timeout in minutes
        /// </summary>
        [DataMember]
        public int CacheTimeoutInMinutes { get; set; } = 0;
        #endregion

        #region .ctor
        /// <summary>
        /// Cms Component instance
        /// </summary>
        public CmsComponentInstance() { }
        /// <summary>
        /// Cms component instance
        /// </summary>
        /// <param name="instance">Component instance</param>
        /// <param name="definition">Component definition</param>
        /// <param name="site">Site</param>
        public CmsComponentInstance(ComponentInstance instance, Component definition, Site site)
        {
            Id = definition.Id;
            Key = definition.Key;
            UpdateDate = definition.UpdateDate;
            Enabled = definition.Enabled;
            Name = definition.Name;
            Rev = definition.Rev;
            RevComment = definition.RevComment;
            PlaceholderType = definition.PlaceholderType;
            PlaceholderZone = instance.PlaceholderZone;
            LayoutOrder = instance.LayoutOrder;
            ComponentClassType = definition.ComponentClassType;
            DateFrom = instance.DateFrom;
            DateTo = instance.DateTo;
            PreRenderVars = definition.PreRenderVars;
            CacheTimeoutInMinutes = definition.CacheTimeoutInMinutes;
            var definitionLocale = definition.Locales.Get(site.MarketKey, site.CultureKey);
            ViewContentType = definitionLocale.ViewContentType;
            ViewContent = definitionLocale.ViewContent;
            EditorContentType = definitionLocale.EditorContentType;
            EditorContent = definitionLocale.EditorContent;
            Vars = definitionLocale.Vars + instance.Vars;
            if (definition.OutputUrls != null)
            {
                OutputUrls = new List<CmsComponentOutputUrl>();
                foreach(var url in definition.OutputUrls)
                {
                    var iUrl = instance.OutputUrls.FirstOrDefault(i => i.UrlKey == url.UrlKey);
                    OutputUrls.Add(new CmsComponentOutputUrl
                    {
                        UrlKey = url.UrlKey,
                        InternalUrl = url.InternalUrl,
                        RequiredComponentsKeyOnDestination = url.RequiredComponentsKeyOnDestination != null ? new List<string>(url.RequiredComponentsKeyOnDestination) : null,
                        PageKey = iUrl?.PageKey,
                        ExternalUrl = iUrl?.ExternalUrl
                    });
                }
            }
        }
        #endregion

    }
}
