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
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models.Renderer
{
    /// <summary>
    /// Cms Request Data interface
    /// </summary>
    public interface ICmsRequestData
    {
        /// <summary>
        /// Current HttpContext
        /// </summary>
        HttpContext Context { get; set; }
        /// <summary>
        /// Cms Page Model
        /// </summary>
        CmsPageModel Page { get; set; }
        /// <summary>
        /// Cms Url Route
        /// </summary>
        CmsUrlMatch Route { get; set; }
        /// <summary>
        /// Component Count
        /// </summary>
        int ComponentCount { get; set; }
        /// <summary>
        /// Create View
        /// </summary>
        bool CreateView { get; set; }
    }
}
