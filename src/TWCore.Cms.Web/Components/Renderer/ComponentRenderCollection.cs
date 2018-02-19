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

namespace TWCore.Cms.Web.Components.Renderer
{
    /// <inheritdoc />
    /// <summary>
    /// Component Render Collection
    /// </summary>
    public class ComponentRenderCollection : List<ComponentRenderItem>
    {
        private const string HeaderTag = "[[HEADER]]";
        private const string ContentTagStart = "[[CONTENT:";
        private const string ContentTagEnd = "]]";
        private const string FooterTag = "[[FOOTER]]";

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; }

        #region .ctor
        /// <inheritdoc />
        /// <summary>
        /// Component Render Collection
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentRenderCollection() { }
        /// <inheritdoc />
        /// <summary>
        /// Component Render Collection
        /// </summary>
        /// <param name="content">Content value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentRenderCollection(string content)
        {
            Content = content;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get Component Render Collection
        /// </summary>
        /// <param name="content">Content value</param>
        /// <returns>ComponentRenderCollection instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComponentRenderCollection GetComponentRenderCollection(string content)
        {
            var itemsToRender = new ComponentRenderCollection(content);

            var headerIndex = content.FastIndexOf(HeaderTag);
            if (headerIndex > -1)
                itemsToRender.Add(new ComponentRenderItem(ComponentPlaceholderRenderType.Header, null, HeaderTag));

            var contentsIndex = content.FastIndexOf(ContentTagStart);
            while (contentsIndex > -1)
            {
                var nIndex = content.FastIndexOf(ContentTagEnd, contentsIndex);
                if (nIndex > 0)
                {
                    var name = content.SubstringIndex(contentsIndex + HeaderTag.Length, nIndex);
                    var replaceVar = content.SubstringIndex(contentsIndex, nIndex + 2);
                    itemsToRender.Add(new ComponentRenderItem(ComponentPlaceholderRenderType.Content, name, replaceVar));
                }
                contentsIndex = content.FastIndexOf(ContentTagStart, nIndex);
            }

            var footerIndex = content.FastIndexOf(FooterTag);
            if (footerIndex > -1)
                itemsToRender.Add(new ComponentRenderItem(ComponentPlaceholderRenderType.Footer, null, FooterTag));

            return itemsToRender;
        }
        #endregion
    }
}
