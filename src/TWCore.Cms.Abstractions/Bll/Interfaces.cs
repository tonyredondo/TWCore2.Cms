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

using System.Threading.Tasks;
using TWCore.Cms.Models;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms.Bll
{
    /// <summary>
    /// Bll Cms interface
    /// </summary>
    public interface IBllCms
    {
        /// <summary>
        /// Prepare site routes
        /// </summary>
        Task PrepareSiteRoutesAsync();
        /// <summary>
        /// Get runtime page model from url
        /// </summary>
        /// <param name="url">Url value</param>
        /// <returns>CmsPageRequest instance</returns>
        CmsPageRequest GetRuntimePageModel(string url);
        /// <summary>
        /// Get runtime page model
        /// </summary>
        /// <param name="scheme">Scheme</param>
        /// <param name="hostname">Hostname</param>
        /// <param name="port">Port</param>
        /// <param name="path">Path</param>
        /// <returns>CmsPageRequest instance</returns>
        CmsPageRequest GetRuntimePageModel(string scheme, string hostname, int port, string path); 
    }
}
