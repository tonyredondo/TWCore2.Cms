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
using System.Runtime.CompilerServices;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models.Renderer
{
    /// <summary>
    /// Component Model
    /// </summary>
    public class ComponentModel
    {
        #region Properties
        /// <summary>
        /// Components
        /// </summary>
        public List<CmsComponentInstance> Components { get; set; }
        /// <summary>
        /// Styles
        /// </summary>
        public ValueCollection Styles { get; set; }
        /// <summary>
        /// Scripts
        /// </summary>
        public ValueCollection Scripts { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public ValueDictionary.ValueDictionaryIndexed Data { get; set; }
        /// <summary>
        /// Page Data
        /// </summary>
        public ValueDictionary.ValueDictionaryIndexed PageData { get; set; }
        /// <summary>
        /// View name
        /// </summary>
        public string View { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Component Model
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentModel()
        {
            Components = new List<CmsComponentInstance>();
            Styles = new ValueCollection();
            Scripts = new ValueCollection();
        }
        /// <summary>
        /// Component Model
        /// </summary>
        /// <param name="page">Cms Page Model</param>
        /// <param name="instance">Cms Component instance</param>
        /// <param name="view">View name</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentModel(CmsPageModel page, CmsComponentInstance instance, string view)
        {
            Components = new List<CmsComponentInstance>();
            if (instance?.Vars != null)
            {
                Data = instance.Vars.Data?.Add(null).GetIndexer();
                Scripts = instance.Vars.Scripts != null ? instance.Vars.Add(null).Scripts : new ValueCollection();
                Styles = instance.Vars.Styles != null ? instance.Vars.Add(null).Styles : new ValueCollection();
            }
            else
            {
                Styles = new ValueCollection();
                Scripts = new ValueCollection();
            }
            PageData = page?.Data?.GetIndexer();
            View = view;
        }
        #endregion
    }
}
