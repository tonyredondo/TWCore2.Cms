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

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Cms Url Binding Collection
    /// </summary>
    [DataContract]
    public class CmsUrlBindingCollection : List<CmsUrlBinding>
    {
        /// <summary>
        /// Get Url Match
        /// </summary>
        /// <param name="scheme">Scheme</param>
        /// <param name="hostname">Hostname</param>
        /// <param name="port">Port</param>
        /// <param name="path">Path</param>
        /// <returns>CmsUrlMatch instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CmsUrlMatch GetMatch(string scheme, string hostname, int port, string path)
        {
            var candidates = FindAll(i => (i.Port <= 0 || i.Port == port) &&
                string.Equals(i.Scheme, scheme, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(i.Hostname, hostname, StringComparison.OrdinalIgnoreCase));

            foreach (var item in candidates)
            {
                var value = item.Match(path);
                if (value != null)
                    return value;
            }
            return null;
        }
    }
}
