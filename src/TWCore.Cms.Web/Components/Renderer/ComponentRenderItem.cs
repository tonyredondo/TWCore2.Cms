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

using System.Runtime.CompilerServices;

namespace TWCore.Cms.Web.Components.Renderer
{
    /// <summary>
    /// Component Render Item
    /// </summary>
    public class ComponentRenderItem
    {
        /// <summary>
        /// Placeholder type
        /// </summary>
        public ComponentPlaceholderRenderType PlaceholderType { get; set; }
        /// <summary>
        /// Placeholder name
        /// </summary>
        public string PlaceholderName { get; set; }
        /// <summary>
        /// Replace Var value
        /// </summary>
        public string ReplaceVar { get; set; }

        #region .ctor
        /// <summary>
        /// Component Render Item
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentRenderItem() { }
        /// <summary>
        /// Component Render Item
        /// </summary>
        /// <param name="placeholderType">Placeholder type</param>
        /// <param name="placeholderName">Placeholder name</param>
        /// <param name="replaceVar">Replace var</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentRenderItem(ComponentPlaceholderRenderType placeholderType, string placeholderName, string replaceVar)
        {
            PlaceholderType = placeholderType;
            PlaceholderName = placeholderName;
            ReplaceVar = replaceVar;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Replace the placeholder with the content
        /// </summary>
        /// <param name="source">Source value</param>
        /// <param name="value">New value</param>
        /// <returns>Replace result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Replace(string source, string value)
        {
            return source.Replace(ReplaceVar, value);
        }
        #endregion
    }
}
