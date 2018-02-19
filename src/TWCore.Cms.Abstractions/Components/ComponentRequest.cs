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

using TWCore.Cms.Entities;
using TWCore.Cms.Entities.Pages;
using TWCore.Cms.Models;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms.Components
{
    /// <summary>
    /// Component Request
    /// </summary>
    public class ComponentRequest
    {
        /// <summary>
        /// Page Request
        /// </summary>
        public CmsPageRequest PageRequest { get; set; }
        /// <summary>
        /// Component definition
        /// </summary>
        public Component Component { get; set; }
        /// <summary>
        /// Component instance
        /// </summary>
        public ComponentInstance Instance { get; set; }

        #region .ctor
        /// <inheritdoc />
        /// <summary>
        /// Component Request
        /// </summary>
        public ComponentRequest() { }
        /// <summary>
        /// Component Request
        /// </summary>
        public ComponentRequest(CmsPageRequest pageRequest, Component component, ComponentInstance instance)
        {
            PageRequest = pageRequest;
            Component = component;
            Instance = instance;
        }
        #endregion
    }
}
