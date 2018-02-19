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
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Cms Url binding
    /// </summary>
    [DataContract]
    public class CmsUrlBinding
    {
        #region Statics
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string RouteParamPattern = @"({([1-9a-z?]*)*})";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string RouteRequiredParamReplacePattern = @"([a-z1-9 -.$\[\]]+)";
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private const string RouteOptionalParamReplacePattern = @"([a-z1-9 -.$\[\]]*)";

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Regex RouteParamRegex = new Regex(RouteParamPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        #endregion

        #region Fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _matchRoute;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<string> _routeParameters = new List<string>();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Regex _rgxMatchRoute;
        #endregion

        #region Properties
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
        /// Route
        /// </summary>
        [DataMember]
        public string Route { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Cms Url binding
        /// </summary>
        public CmsUrlBinding() { }
        /// <summary>
        /// Cms Url binding
        /// </summary>
        /// <param name="value">Value</param>
        public CmsUrlBinding(string value)
        {
            var builder = new UriBuilder(value);
            Scheme = builder.Scheme;
            Hostname = builder.Host;
            Port = builder.Port;
            Route = builder.Path;
            Load();
        }
        /// <summary>
        /// Cms Url binding
        /// </summary>
        /// <param name="urlBinding">Url binding</param>
        /// <param name="route">Route</param>
        public CmsUrlBinding(UrlBinding urlBinding, string route)
        {
            Scheme = urlBinding.Scheme;
            Hostname = urlBinding.Hostname;
            Port = urlBinding.Port;
            Route = urlBinding.Path;
			if (!Route.EndsWith("/", StringComparison.Ordinal))
                Route += "/";

            if (route?.StartsWith("/") == true)
                Route += route.Substring(1);
            else
                Route += route;

            Load();
        }
        /// <summary>
        /// Cms Url binding
        /// </summary>
        /// <param name="scheme">Scheme</param>
        /// <param name="hostname">Hostname</param>
        /// <param name="port">Port</param>
        /// <param name="route">Route</param>
        public CmsUrlBinding(string scheme, string hostname, int port, string route)
        {
            Scheme = scheme;
            Hostname = hostname;
            Port = port;
            Route = route;
            Load();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get url
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Uri GetUri() 
            => new UriBuilder(Scheme, Hostname, Port, Route).Uri;
        /// <summary>
        /// Load
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Load()
        {
            _matchRoute = Route;
            foreach (Match match in RouteParamRegex.Matches(Route))
            {
                if (!match.Success || match.Value.Length == 0) continue;
                _routeParameters.Add(match.Value);
                _matchRoute = _matchRoute.Replace(match.Value, 
                    match.Value.EndsWith("?}", StringComparison.Ordinal) ? 
                        RouteOptionalParamReplacePattern : RouteRequiredParamReplacePattern);
            }
            _matchRoute = _matchRoute.Replace("/", @"(\/*)");
            _rgxMatchRoute = new Regex(_matchRoute, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
        /// <summary>
        /// Match
        /// </summary>
        /// <param name="route">Route</param>
        /// <returns>CmsUrlMatch instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CmsUrlMatch Match(string route)
        {
            if (_rgxMatchRoute == null)
                Load();
            route = Uri.UnescapeDataString(route);
            var match = _rgxMatchRoute.Match(route);
            if (match.Length != route.Length)
                return null;

            var response = new Dictionary<string, string>();
            var lstGroups = new List<Group>();
            for (var idx = 1; idx < match.Groups.Count; idx++)
            {
                var group = match.Groups[idx];
                if (group.Value != "/" && group.Value != "//")
                    lstGroups.Add(group);
            }

            var pIdx = 0;
            foreach (var group in lstGroups)
            {
                if (lstGroups.Count < _routeParameters.Count && _routeParameters[pIdx].EndsWith("?}", StringComparison.Ordinal))
                    pIdx++;
                if (pIdx < _routeParameters.Count)
                {
                    var param = _routeParameters[pIdx];
                    var value = group.Value;
                    param = param.Substring(1, param.Length - 2).Replace("?", "");
                    response[param] = !string.IsNullOrEmpty(value) ? value : null;
                    pIdx++;
                }
            }
            return new CmsUrlMatch(this, response);
        }
        #endregion

        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator CmsUrlBinding(string value)
            => new CmsUrlBinding(value);
        #endregion
    }
}
