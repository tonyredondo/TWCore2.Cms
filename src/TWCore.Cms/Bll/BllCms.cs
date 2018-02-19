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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TWCore.Cms.Dal;
using TWCore.Cms.Entities;
using TWCore.Cms.Entities.Pages;
using TWCore.Cms.Models;
using TWCore.Security;

namespace TWCore.Cms.Bll
{
    /// <inheritdoc />
    /// <summary>
    /// Cms BLL
    /// </summary>
    public class BllCms : IBllCms
    {
        private readonly Random _rnd = new Random();
        private readonly IDalCms _dal;
        private readonly ConcurrentDictionary<(string Scheme, string Hostname, int Port, string Path), CmsPageRequest> _requestsCache 
            = new ConcurrentDictionary<(string Scheme, string Hostname, int Port, string Path), CmsPageRequest>();
        private CmsPage[] _pages;

        #region .ctor
        /// <summary>
        /// Cms BLL
        /// </summary>
        /// <param name="dal">Cms Dal</param>
        public BllCms(IDalCms dal)
            => _dal = dal;
        #endregion

        #region Public Methods
        /// <summary>
        /// Prepare site routes
        /// </summary>
        public void PrepareSiteRoutes()
            => PrepareSiteRoutesAsync().WaitAsync();
        /// <inheritdoc />
        /// <summary>
        /// Prepare site routes
        /// </summary>
        public async Task PrepareSiteRoutesAsync()
        {
            if (_pages != null) return;

            try
            {
                var allComponents = (await _dal.Components.GetAllAsync().ConfigureAwait(false))
                    .GroupBy(k => k.KeyWithRev).ToDictionary(k => k.Key, v => v.First());
                var allStyles = (await _dal.Styles.GetAllAsync().ConfigureAwait(false))
                    .ToDictionary(k => k.Key);
                var allScripts = (await _dal.Scripts.GetAllAsync().ConfigureAwait(false))
                    .ToDictionary(k => k.Key);
                var allMarkets = (await _dal.Markets.GetAllAsync().ConfigureAwait(false))
                    .ToDictionary(k => k.Key);
                var allCultures = (await _dal.Cultures.GetAllAsync().ConfigureAwait(false))
                    .ToDictionary(k => k.Key);
                var allSites = (await _dal.Sites.GetAllAsync().ConfigureAwait(false))
                    .ToArray();
                var allPagesGroups = (await _dal.PagesGroups.GetAllAsync().ConfigureAwait(false))
                    .ToArray();
                var allPages = (await _dal.Pages.GetAllAsync().ConfigureAwait(false))
                    .ToArray();

                var cmsPages = new List<CmsPage>();

                Core.Log.Verbose("Preparing site routes");

                foreach (var site in allSites)
                {
                    #region Site
                    if (!site.Enabled)
                    {
                        Core.Log.Warning("The Site '{0}' is not enabled", site.Key);
                        continue;
                    }
                    if (!site.Published)
                    {
                        Core.Log.InfoBasic("The Site '{0}' has not been published", site.Key);
                        continue;
                    }
                    Core.Log.InfoBasic("Loading site: {0}", site.Key);
                    #endregion

                    #region Market
                    if (!allMarkets.TryGetValue(site.MarketKey, out var market))
                    {
                        Core.Log.Error("The Market '{0}' for site '{1}' can't be found, skipping site.", site.MarketKey, site.Key);
                        continue;
                    }
                    if (!market.Enabled)
                    {
                        Core.Log.Error("The Market '{0}' is disabled, skipping site.", market.Key);
                        continue;
                    }
                    Core.Log.InfoBasic("Market: {0}", market.Key);
                    #endregion

                    #region Culture
                    if (!allCultures.TryGetValue(site.CultureKey, out var culture))
                    {
                        Core.Log.Error("The Culture '{0}' for site '{1}' can't be found, skipping site.", site.CultureKey, site.Key);
                        continue;
                    }
                    if (!culture.Enabled)
                    {
                        Core.Log.Error("The Culture '{0}' is disabled, skipping site.", culture.Key);
                        continue;
                    }
                    Core.Log.InfoBasic("Culture: {0}", culture.Key);
                    #endregion

                    var pagesGroups = allPagesGroups.Where(pg => site.PagesGroups.Contains(pg.Key)).ToArray();
                    Core.Log.InfoBasic("Pages Groups Count: {0}", pagesGroups.Length);

                    foreach (var pGroup in pagesGroups)
                    {
                        if (!pGroup.Enabled)
                        {
                            Core.Log.Warning("The Pages Group '{0}' is not enabled", pGroup.Key);
                            continue;
                        }
                        Core.Log.InfoBasic("Group: {0}", pGroup.Key);

                        var pages = allPages.Where(p => pGroup.PagesKey.Contains(p.Key)).ToArray();
                        Core.Log.InfoBasic("Pages in the Group: {0}", pages.Length);

                        foreach (var page in pages)
                        {
                            if (!page.Enabled)
                            {
                                Core.Log.Warning("The Page '{0}' is not enabled", page.Key);
                                continue;
                            }

                            Core.Log.InfoBasic("Loading Page: {0}", page.Key);
                            var cPage = page.DeepClone();
                            var pageInstances = cPage.Instances.RemoveAllBut(market.Key, culture.Key, pGroup.Key, site.Key)
                                .Where(i => i.PublishStatus == PublishStatus.Published)
                                .ToArray();

                            if (pageInstances.Length == 0)
                            {
                                Core.Log.Warning("There aren't any page instance for Market={0}, Culture={1}, Group={2}, Site={3}, skipping page.", market.Key, culture.Key, pGroup.Key, site.Key);
                                continue;
                            }

                            var pageInstance = cPage.SelectionMode == PageInstanceSelectionMode.First ? pageInstances[0] : pageInstances[_rnd.Next(pageInstances.Length)];
                            Core.Log.InfoBasic("Page instance for Market={0}, Culture={1}, Group={2}, Site={3} loaded.", pageInstance.MarketKey, pageInstance.CultureKey, pageInstance.PagesGroupKey, pageInstance.SiteKey);

                            var cSite = site.DeepClone();
                            var cGroup = pGroup.DeepClone();

                            #region Creating Page Variables
                            var currentPageVariables = new Variables();
                            var cGroupVariables = pGroup.LocalizedVars.RemoveAllBut(site.MarketKey, site.CultureKey);
                            currentPageVariables = market.Vars + culture.Vars + cGroupVariables?.Vars + site.Vars + pageInstance.Vars;
                            #endregion

                            var cmsPage = new CmsPage()
                            {
                                Page = cPage,
                                PagesGroup = cGroup,
                                Site = cSite,
                                Culture = culture,
                                Market = market,
                                CurrentPageVariables = currentPageVariables,
                            };

                            var componentHash = new StringBuilder();

                            #region Site
                            if (cSite.Header != null)
                                cmsPage.Header = GetCmsComponentInstance(cSite.Header, allComponents, cSite, cmsPage, componentHash);
                            if (cSite.Footer != null)
                                cmsPage.Footer = GetCmsComponentInstance(cSite.Footer, allComponents, cSite, cmsPage, componentHash);
                            #endregion

                            #region Uris
                            Core.Log.InfoBasic("Setting uris.");
                            cmsPage.Uris = new CmsUrlBindingCollection();
                            foreach (var baseUrl in site.UrlBindings)
                            {
                                foreach (var route in pageInstance.Routes)
                                {
                                    cmsPage.Uris.Add(new CmsUrlBinding(baseUrl, route));
                                }
                            }
                            #endregion

                            #region Scripts
                            Core.Log.InfoBasic("Setting scripts.");
                            cmsPage.Scripts = new Dictionary<string, Script>();
                            foreach (var script in currentPageVariables.Scripts)
                            {
                                if (allScripts.TryGetValue(script.SValue, out var scr))
                                    cmsPage.Scripts[script.SValue] = scr;
                                else
                                {
                                    cmsPage.Messages.Add(new MessageModel(MessageType.Warning, string.Format("The Script '{0}' wasn't found, skipping it.", script.Value)));
                                    Core.Log.Warning("The Script '{0}' wasn't found, skipping it.", script.Value);
                                }
                            }
                            #endregion

                            #region Styles
                            Core.Log.InfoBasic("Setting styles.");
                            cmsPage.Styles = new Dictionary<string, Stylesheet>();
                            foreach (var style in currentPageVariables.Styles)
                            {
                                if (allStyles.TryGetValue(style.SValue, out var stl))
                                    cmsPage.Styles[style.SValue] = stl;
                                else
                                {
                                    cmsPage.Messages.Add(new MessageModel(MessageType.Warning, string.Format("The Stylesheet '{0}' wasn't found, skipping it.", style.Value)));
                                    Core.Log.Warning("The Stylesheet '{0}' wasn't found, skipping it.", style.Value);
                                }
                            }
                            #endregion

                            #region Components
                            Core.Log.InfoBasic("Setting components.");
                            cmsPage.ParentComponent = GetCmsComponentInstance(pageInstance.ParentComponent, allComponents, cSite, cmsPage, componentHash);
                            #endregion

                            #region Page Hash
                            cmsPage.Hash = 33 +
                                site.Key + site.Rev + site.UpdateDate.Ticks +
                                market.Key + market.UpdateDate.Ticks +
                                culture.Key + culture.UpdateDate.Ticks +
                                cGroup.Key + cGroup.UpdateDate.Ticks +
                                cPage.Key + cPage.Rev + cPage.UpdateDate.Ticks + componentHash;
                            cmsPage.Hash = cmsPage.Hash.GetHashSHA256();
                            #endregion

                            #region Page Path
                            cmsPage.Path = $"{site.Key}/{pGroup.Key}/{page.Key}/{cmsPage.Hash}/";
                            #endregion

                            #region Page View Path
                            cmsPage.ViewPath = cmsPage.Path + "Page" + ".cshtml";
                            #endregion

                            Core.Log.InfoBasic("Page loaded.");
                            cmsPages.Add(cmsPage);
                        }
                    }
                }

                Core.Log.InfoBasic("All pages loaded: {0}", cmsPages.Count);
                _pages = cmsPages.ToArray();
            }
            catch (Exception ex)
            {
                Core.Log.Write(ex);
            }
        }
        /// <inheritdoc />
        /// <summary>
        /// Get runtime page model from url
        /// </summary>
        /// <param name="url">Url value</param>
        /// <returns>CmsPageRequest instance</returns>
        public CmsPageRequest GetRuntimePageModel(string url)
        {
            var urib = new UriBuilder(url);
            return GetRuntimePageModel(urib.Scheme, urib.Host, urib.Port, urib.Path);
        }
        /// <inheritdoc />
        /// <summary>
        /// Get runtime page model
        /// </summary>
        /// <param name="scheme">Scheme</param>
        /// <param name="hostname">Hostname</param>
        /// <param name="port">Port</param>
        /// <param name="path">Path</param>
        /// <returns>CmsPageRequest instance</returns>
        public CmsPageRequest GetRuntimePageModel(string scheme, string hostname, int port, string path)
        {
            return _requestsCache.GetOrAdd((scheme, hostname, port, path), mKey =>
            {
                if (_pages == null)
                    PrepareSiteRoutes();
                foreach (var page in _pages)
                {
                    var match = page.Uris.GetMatch(mKey.Scheme, mKey.Hostname, mKey.Port, mKey.Path);
                    if (match != null)
                        return new CmsPageRequest { Page = page, Route = match };
                }
                foreach (var page in _pages)
                {
                    var match = page.Uris.GetMatch(mKey.Scheme, "*", mKey.Port, mKey.Path);
                    if (match != null)
                        return new CmsPageRequest { Page = page, Route = match };
                }
                return null;
            });
        }
        #endregion

        #region Private Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CmsComponentInstance GetCmsComponentInstance(ComponentInstance componentInstance, Dictionary<string, Component> allComponents, Site site, CmsPage cmsPage, StringBuilder componentHash)
        {
            if (componentInstance == null) return null;

            if (allComponents.TryGetValue(componentInstance.ComponentKeyAndRev, out var component))
            {
                Core.Log.InfoBasic("Loading component: {0}", componentInstance.ComponentKeyAndRev);
                var cmsComponent = new CmsComponentInstance(componentInstance, component, site);
                if (componentInstance.Children != null)
                {
                    cmsComponent.Children = new List<CmsComponentInstance>();
                    foreach(var childInstance in componentInstance.Children)
                    {
                        var childCmsInstance = GetCmsComponentInstance(childInstance, allComponents, site, cmsPage, componentHash);
                        if (childCmsInstance != null)
                            cmsComponent.Children.Add(childCmsInstance);
                    }
                }
                componentHash.Append(cmsComponent.Key + cmsComponent.Rev + cmsComponent.ComponentClassType + cmsComponent.UpdateDate + cmsComponent.DateFrom + cmsComponent.DateTo + cmsComponent.PlaceholderZone + cmsComponent.LayoutOrder);
                return cmsComponent;
            }

            cmsPage.Messages.Add(new MessageModel(MessageType.Warning, string.Format("The Component '{0}:{1}' wasn't found, skipping it.", componentInstance.ComponentKey, componentInstance.ComponentRev)));
            Core.Log.Warning("The Component '{0}:{1}' wasn't found, skipping it.", componentInstance.ComponentKey, componentInstance.ComponentRev);
            return null;
        }
        #endregion


        /*
        public void Render(CmsPageRequest request, TextWriter writer)
        {
            if (request != null)
            {
                if (request.Page.Layout != null)
                {
                    ExtractTokens(request.Page.Layout.Html, writer, (token, twriter) =>
                    {
                        RenderToken(request, token, twriter);
                    });
                }
            }
        }

        static void RenderToken(CmsPageRequest request, string token, TextWriter writer)
        {
            if (token.StartsWith("Culture."))
            {
                token = token.Substring(8);
                if (token == nameof(request.Page.Culture.FirstDayOfWeek))
                    writer.Write(request.Page.Culture.FirstDayOfWeek);
                if (token == nameof(request.Page.Culture.IsoTag))
                    writer.Write(request.Page.Culture.IsoTag);
                if (token == nameof(request.Page.Culture.Key))
                    writer.Write(request.Page.Culture.Key);
                if (token == nameof(request.Page.Culture.LongDateFormat))
                    writer.Write(request.Page.Culture.LongDateFormat);
                if (token == nameof(request.Page.Culture.LongTimeFormat))
                    writer.Write(request.Page.Culture.LongTimeFormat);
                if (token == nameof(request.Page.Culture.ShortDateFormat))
                    writer.Write(request.Page.Culture.ShortDateFormat);
                if (token == nameof(request.Page.Culture.ShortTimeFormat))
                    writer.Write(request.Page.Culture.ShortTimeFormat);
            }
            else if (token.StartsWith("Market."))
            {
                token = token.Substring(7);
                if (token == nameof(request.Page.Market.IsoTag))
                    writer.Write(request.Page.Market.IsoTag);
                if (token == nameof(request.Page.Market.Key))
                    writer.Write(request.Page.Market.Key);
            }
            else if (token.StartsWith("Vars."))
            {
                token = token.Substring(5);
                if (request.Page.Vars.Data.TryGetValue(token, out var str))
                    writer.Write(str.Value);
                else if (request.Page.Vars.Data.TryGetValue(token, out var num))
                    writer.Write(num.Value);
                else if (request.Page.Vars.Data.TryGetValue(token, out var flag))
                    writer.Write(flag.Value);
            }
            else if (token.StartsWith("Site."))
            {
                token = token.Substring(5);
                if (token == "HeaderTags")
                {
                    foreach (var style in request.Page.Styles)
                    {
                        if (style.Type == StylesheetType.Css)
                            writer.Write(string.Format("<link type='text/css' href='content/{0}' rel='stylesheet' media='{1}' />", style.Source, style.Media.ToString().ToLowerInvariant()));
                        else if (style.Type == StylesheetType.InlineCss)
                            writer.Write(string.Format("<style type='text/css' media='{1}'>{0}</style>", style.Source, style.Media.ToString().ToLowerInvariant()));
                    }
                    foreach (var script in request.Page.Scripts)
                    {
                        if (script.Type == ScriptType.Js)
                            writer.Write(string.Format("<script type='text/javascript' src='content/{0}' {1}></script>", script.Source, script.LoadType == ScriptLoad.Normal ? string.Empty : script.LoadType == ScriptLoad.Async ? "async" : "defer"));
                        else if (script.Type == ScriptType.InlineJs)
                            writer.Write(string.Format("<script type='text/javascript' {1}>{0}</script>", script.Source, script.LoadType == ScriptLoad.Normal ? string.Empty : script.LoadType == ScriptLoad.Async ? "async" : "defer"));
                    }
                }
            }
            else if (token.StartsWith("Zones."))
            {
                token = token.Substring(6);
                var sset = new SortedSet<ComponentResult>(ComponentResultComparer.Default);
                request.Page.Page.Instances[0].Components.Sort((a, b) => a.PipelineOrder.CompareTo(b.PipelineOrder));
                foreach (var cmp in request.Page.Page.Instances[0].Components)
                {
                    var component = request.Page.Components[cmp.ComponentKey];
                    //if (!string.IsNullOrWhiteSpace(component.PipelineClassType))
                    //{
                    //    var pipelineClass = Core.GetType(component.PipelineClassType);
                    //    if (pipelineClass != null)
                    //    {
                    //        var cmpInstance = (IComponent)Activator.CreateInstance(pipelineClass);
                    //        cmpInstance.Load(new ComponentRequest(request, component, cmp));
                    //    }
                    //    else
                    //        Core.Log.Warning("The Component Type: '{0}' couldn't be created.", component.PipelineClassType);
                    //}
                    //Razor over view then
                    sset.Add(new ComponentResult { Html = cmp.InstanceValue, Order = cmp.LayoutOrder });
                }
                foreach (var item in sset)
                    writer.Write(item.Html);
            }
        }

        class ComponentResult
        {
            public string Html { get; set; }
            public int Order { get; set; }
        }
        class ComponentResultComparer : IComparer<ComponentResult>
        {
            public static readonly ComponentResultComparer Default = new ComponentResultComparer();
            public int Compare(ComponentResult x, ComponentResult y) => x.Order.CompareTo(y.Order);
        }
        

        private static Tuple<List<string>, List<string>> ExtractTokens(string value)
        {
            var lstToken = new List<string>();
            var lstValues = new List<string>();
            var lastIndex = 0;
            while (lastIndex < value.Length)
            {
                var sIndex = value.IndexOf("@", lastIndex, StringComparison.Ordinal);
                var nIndex = sIndex + 1;
                if (sIndex < 0 || value.Length <= nIndex)
                {
                    lstValues.Add(value.Substring(lastIndex, value.Length - lastIndex));
                    break;
                }
                if (value[nIndex] == '{')
                {
                    var eIndex = value.IndexOf("}", nIndex, StringComparison.Ordinal);
                    if (eIndex >= 0)
                    {
                        lstToken.Add(value.Substring(sIndex + 2, eIndex - sIndex - 2));
                        lstValues.Add(value.Substring(lastIndex, sIndex - lastIndex));
                    }
                    lastIndex = eIndex + 1;
                }
                else
                    lastIndex = nIndex;
            }
            return Tuple.Create(lstToken, lstValues);
        }
        private static void ExtractTokens(string value, TextWriter result, Action<string, TextWriter> onTokenAction)
        {
            var lastIndex = 0;
            while (lastIndex < value.Length)
            {
                var sIndex = value.IndexOf("@", lastIndex, StringComparison.Ordinal);
                var nIndex = sIndex + 1;
                if (sIndex < 0 || value.Length <= nIndex)
                {
                    result.Write(value.Substring(lastIndex, value.Length - lastIndex));
                    break;
                }
                if (value[nIndex] == '{')
                {
                    var eIndex = value.IndexOf("}", nIndex, StringComparison.Ordinal);
                    if (eIndex >= 0)
                    {
                        result.Write(value.Substring(lastIndex, sIndex - lastIndex));
                        onTokenAction?.Invoke(value.Substring(sIndex + 2, eIndex - sIndex - 2), result);
                    }
                    lastIndex = eIndex + 1;
                }
                else
                    lastIndex = nIndex;
            }
        }
        */
    }
}
