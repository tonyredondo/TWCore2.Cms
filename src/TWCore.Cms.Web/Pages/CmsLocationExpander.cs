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

using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TWCore.Cms.Web.Pages
{
    /// <inheritdoc />
    /// <summary>
    /// Cms Location Expander
    /// </summary>
    public class CmsLocationExpander : IViewLocationExpander
    {
        #region Public Methods
        /// <inheritdoc />
        /// <summary>
        /// Expand View Locations
        /// </summary>
        /// <param name="context">Expander context</param>
        /// <param name="viewLocations">View locations</param>
        /// <returns>New View Locations</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
            => new[] { "/CmsViews/" + context.ViewName + ".cshtml" }.Concat(viewLocations);

        /// <inheritdoc />
        /// <summary>
        /// Populate Values
        /// </summary>
        /// <param name="context">Expander context</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
        #endregion
    }
}
