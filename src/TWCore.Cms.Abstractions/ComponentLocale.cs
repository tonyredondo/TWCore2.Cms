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
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace TWCore.Cms
{
    /// <inheritdoc />
    /// <summary>
    /// Component Locale definition
    /// </summary>
    [DataContract]
    public class ComponentLocale : LocalizedVariables
    {
        /// <summary>
        /// Type of the content in View mode
        /// </summary>
        [DataMember]
        public ComponentContentType ViewContentType { get; set; }
        /// <summary>
        /// Content in View mode
        /// </summary>
        [DataMember]
        public string ViewContent { get; set; }
        /// <summary>
        /// Type of the content in Editor mode
        /// </summary>
        [DataMember]
        public ComponentContentType EditorContentType { get; set; }
        /// <summary>
        /// Content in Editor mode
        /// </summary>
        [DataMember]
        public string EditorContent { get; set; }
        
        #region .ctor
        /// <inheritdoc />
        /// <summary>
        /// Component Locale definition
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentLocale() { }
        /// <inheritdoc />
        /// <summary>
        /// Component Locale definition
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentLocale(string marketKey, string cultureKey, Variables vars) : base(marketKey, cultureKey, vars)
        {
        }
        /// <inheritdoc />
        /// <summary>
        /// Component Locale definition
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentLocale(Variables vars) : base(vars)
        {
        }
        #endregion
    }
}
