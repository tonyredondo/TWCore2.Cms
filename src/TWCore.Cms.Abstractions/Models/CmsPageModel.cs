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
using System.Collections.ObjectModel;
using TWCore.Cms.Entities;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Cms Page Model
    /// </summary>
    public class CmsPageModel
    {
        #region Properties
        /// <summary>
        /// Behavior pipeline
        /// </summary>
        public ReadOnlyCollection<string> BehaviorPipeline { get; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Market
        /// </summary>
        public string Market { get; }
        /// <summary>
        /// Culture
        /// </summary>
        public string Culture { get; }
        /// <summary>
        /// Group
        /// </summary>
        public string Group { get; }
        /// <summary>
        /// Site
        /// </summary>
        public string Site { get; }
        /// <summary>
        /// Keywords
        /// </summary>
        public string Keywords { get; }
        /// <summary>
        /// Canonical url
        /// </summary>
        public string CanonicalUrl { get; }
        /// <summary>
        /// Header
        /// </summary>
        public CmsComponentInstance Header { get; }
        /// <summary>
        /// Footer
        /// </summary>
        public CmsComponentInstance Footer { get; }
        /// <summary>
        /// Parent Component
        /// </summary>
        public CmsComponentInstance ParentComponent { get; }
        /// <summary>
        /// Hash
        /// </summary>
        public string Hash { get; }
        /// <summary>
        /// Path
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// View Path
        /// </summary>
        public string ViewPath { get; }
        /// <summary>
        /// Value dictionary Data
        /// </summary>
        public ValueDictionary Data { get; }
        /// <summary>
        /// Styles
        /// </summary>
        public List<Stylesheet> Styles { get; }
        /// <summary>
        /// Scripts
        /// </summary>
        public List<Script> Scripts { get; }
        #endregion

        #region .ctor
        /// <summary>
        /// Cms Page Model
        /// </summary>
        /// <param name="page">CmsPage instance</param>
        public CmsPageModel(CmsPage page)
        {
            var pageInstance = page.Page.Instances[0];
            Title = pageInstance.Title;
            Market = page.Market.IsoTag;
            Culture = page.Culture.IsoTag;
            Group = page.PagesGroup.Key;
            Site = page.Site.Key;
            Header = page.Header;
            Footer = page.Footer;
            ParentComponent = page.ParentComponent;
            Keywords = pageInstance.Keywords;
            CanonicalUrl = pageInstance.CanonicalUrl;
            BehaviorPipeline = page.Page.BehaviorPipelineTypes?.AsReadOnly();
            Hash = page.Hash;
            Path = page.Path;
            ViewPath = page.ViewPath;
            if (page.CurrentPageVariables != null)
            {
                var vars = page.CurrentPageVariables;
                Data = vars.Data?.Add(null);
                if (vars.Styles != null)
                {
                    Styles = new List<Stylesheet>();
                    foreach (var style in vars.Styles)
                        Styles.Add(page.Styles[style.SValue]);
                }
                if (vars.Scripts != null)
                {
                    Scripts = new List<Script>();
                    foreach (var style in vars.Scripts)
                        Scripts.Add(page.Scripts[style.SValue]);
                }
            }
        }
        #endregion
    }
}
