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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models.Renderer
{
    /// <inheritdoc />
    /// <summary>
    /// Cms request data
    /// </summary>
    public class CmsRequestData : ICmsRequestData
    {
        /// <inheritdoc />
        /// <summary>
        /// Http context
        /// </summary>
        public HttpContext Context { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Cms page model
        /// </summary>
        public CmsPageModel Page { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Cms route
        /// </summary>
        public CmsUrlMatch Route { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Component count
        /// </summary>
        public int ComponentCount { get; set; }
        /// <inheritdoc />
        /// <summary>
        /// Gets or Sets Create view
        /// </summary>
        public bool CreateView { get; set; }
    }
}
