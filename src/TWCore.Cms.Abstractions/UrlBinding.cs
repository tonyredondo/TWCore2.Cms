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
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace TWCore.Cms
{
    /// <summary>
    /// Url Binding
    /// </summary>
    [DataContract]
    public class UrlBinding
    {
        /// <summary>
        /// Scheme
        /// </summary>
        [DataMember]
        public string Scheme { get; set; }
        /// <summary>
        /// Hostname
        /// </summary>
        [DataMember]
        public string Hostname { get; set; }
        /// <summary>
        /// Port
        /// </summary>
        [DataMember]
        public int Port { get; set; }
        /// <summary>
        /// Path
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        #region .ctor
        /// <summary>
        /// Url Binding
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UrlBinding() { }
        /// <summary>
        /// Url Binding
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UrlBinding(string value)
        {
            try
            {
                var builder = new UriBuilder(value);
                Scheme = builder.Scheme;
                Hostname = builder.Host;
                Port = builder.Port;
                Path = builder.Path;
            }
            catch
            {
                var idx = 0;
                if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                {
                    Scheme = "http";
                    idx += 7;
                }
                if (value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    Scheme = "https";
                    idx += 8;
                }
                var dotPort = value.IndexOf(':', idx);
                var slashPath = value.IndexOf('/', dotPort);

                Hostname = value.Substring(idx, dotPort - idx);
                Port = int.Parse(value.Substring(dotPort + 1, slashPath - (dotPort + 1)));
                Path = value.Substring(slashPath);
            }
        }
        /// <summary>
        /// Url Binding
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UrlBinding(string scheme, string hostname, int port, string path)
        {
            Scheme = scheme;
            Hostname = hostname;
            Port = port;
            Path = path;
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Get Uri value
        /// </summary>
        /// <returns>Uri instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uri GetUri() 
            => new UriBuilder(Scheme, Hostname, Port, Path).Uri;
        #endregion
        
        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UrlBinding(string value) 
            => new UrlBinding(value);
        #endregion
    }
}
