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

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TWCore.Cms.Models;
using TWCore.Cms.Models.Renderer;
using TWCore.Collections;
using TWCore.Serialization;
using TWCore.Serialization.WSerializer;
using static TWCore.Singleton;

namespace TWCore.Cms.Web.Components.Renderer
{
    /// <inheritdoc />
    /// <summary>
    /// Render Component
    /// </summary>
    public class RenderComponent : ViewComponent
    {
        private static readonly LRU2QCollection<string, List<CmsComponentInstance>> ComponentDictionaryInstancesCache = new LRU2QCollection<string, List<CmsComponentInstance>>();
        private static readonly LRU2QCollection<(int ComponentCount, string ComponentKey, int ComponentRev, string ComponentClassType, string PageModelPath), (string, string, string, bool)> PathsAndOptionsCache 
            = new LRU2QCollection<(int ComponentCount, string ComponentKey, int ComponentRev, string ComponentClassType, string PageModelPath), (string, string, string, bool)>();
        private static readonly ISerializer Serializer = new WBinarySerializer();
        private readonly ICmsRequestData _cmsPageData;

        #region .ctor
        /// <summary>
        /// Render Component
        /// </summary>
        /// <param name="cmsPageData">CmsRequestData </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RenderComponent(ICmsRequestData cmsPageData)
            => _cmsPageData = cmsPageData;

        #endregion

        #region Public Methods
        /// <summary>
        /// Get Paths and Options
        /// </summary>
        /// <param name="componentCount">Component count</param>
        /// <param name="component">Component instance</param>
        /// <param name="pageModel">Page Model</param>
        /// <returns>Paths and options struct</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (string viewFile, string viewPath, string componentsPath, bool noPipeline) GetPathsAndOptions(int componentCount, CmsComponentInstance component, CmsPageModel pageModel)
        {
            return PathsAndOptionsCache.GetOrAdd((componentCount, component.Key, component.Rev, component.ComponentClassType, pageModel.Path), mKey =>
            {
                var global = InstanceOf<Global>();
                var viewFile = mKey.ComponentCount + " - " + mKey.ComponentKey + "-" + mKey.ComponentRev + ".cshtml";
                var viewPath = global.GetView(mKey.PageModelPath + viewFile);
                var componentsPath = viewPath + ".wbin";
                var noPipeline = string.IsNullOrEmpty(mKey.ComponentClassType);
                return (viewFile, viewPath, componentsPath, noPipeline);
            });
        }

        /// <summary>
        /// Get IViewComponentResult
        /// </summary>
        /// <param name="component">Cms Component Instance</param>
        /// <returns>View Component Result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<IViewComponentResult> InvokeAsync(CmsComponentInstance component)
        {
            if (component == null) return Content("Component is Null");

            var (viewFile, viewPath, componentsPath, noPipeline) = GetPathsAndOptions(++_cmsPageData.ComponentCount, component, _cmsPageData.Page);
            var model = new ComponentModel(_cmsPageData.Page, component, viewFile);

            if (!noPipeline)
            {
                var type = Core.GetType(component.ComponentClassType);
                if (type != null)
                {
                    var processor = (IComponentProcessor) Activator.CreateInstance(type);
                    await processor.InvokeAsync(_cmsPageData, model, component).ConfigureAwait(false);
                }
            }

            if (_cmsPageData.CreateView)
            {
                var global = InstanceOf<Global>();
                var addTagHelper = false;
                var content = component.ViewContent;

                if (component.PlaceholderType == ComponentPlaceholderType.Layout)
                {
                    #region Render Layout Placeholder
                    var itemsToRender = ComponentRenderCollection.GetComponentRenderCollection(content);
                    foreach (var item in itemsToRender)
                    {
                        switch (item.PlaceholderType)
                        {
                            case ComponentPlaceholderRenderType.Header:
                                if (_cmsPageData.Page.Header != null)
                                {
                                    model.Components.Add(_cmsPageData.Page.Header);
                                    content = item.Replace(content, "@await Component.InvokeAsync(\"RenderComponent\", Model.Components[" + (model.Components.Count - 1) + "])");
                                }
                                else
                                    content = item.Replace(content, string.Empty);
                                break;

                            case ComponentPlaceholderRenderType.Content:
                                var children = component.Children?.Where(c => c.PlaceholderZone == item.PlaceholderName).ToArray();
                                if (children?.Any() == true)
                                {
                                    var sbContent = new StringBuilder();
                                    foreach (var childComponent in children)
                                    {
                                        model.Components.Add(childComponent);
                                        sbContent.Append("@await Component.InvokeAsync(\"RenderComponent\", Model.Components[" + (model.Components.Count - 1) + "])");
                                    }
                                    content = item.Replace(content, sbContent.ToString());
                                }
                                else
                                    content = item.Replace(content, string.Empty);
                                break;

                            case ComponentPlaceholderRenderType.Footer:
                                if (_cmsPageData.Page.Footer != null)
                                {
                                    model.Components.Add(_cmsPageData.Page.Footer);
                                    content = item.Replace(content, "@await Component.InvokeAsync(\"RenderComponent\", Model.Components[" + (model.Components.Count - 1) + "])");
                                }
                                else
                                    content = item.Replace(content, string.Empty);
                                break;
                        }
                    }
                    #endregion
                }
                else if (noPipeline && !content.Contains("Model.PageData"))
                {
                    if (component.PreRenderVars)
                    {
                        while (true)
                        {
                            var idx = content.FastIndexOf("@Model.Data[\"");
                            if (idx == -1)
                                break;
                            var idx2 = content.FastIndexOf("\"]", idx);

                            var key = content.SubstringIndex(idx + 13, idx2);
                            var value = model.Data[key];
                            content = content.Replace(content.SubstringIndex(idx, idx2 + 2), value.ToString());
                        }
                    }
                    else if (component.CacheTimeoutInMinutes > 0)
                    {
                        addTagHelper = true;
                        content = "<cache expires-after=\"@TimeSpan.FromMinutes(" + component.CacheTimeoutInMinutes + ")\">" + content + "</cache>";
                    }
                }

                if (addTagHelper)
                    content = "@addTagHelper \" *, Microsoft.AspNeTWCore.Mvc.TagHelpers\"\r\n" + content;

                #region Write View file
                lock (component)
                {
                    var path = Path.GetDirectoryName(viewPath);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    File.WriteAllText(viewPath, content);
                    Serializer.SerializeToFile(model.Components, componentsPath);
                    ComponentDictionaryInstancesCache.TryAdd(componentsPath, model.Components);
                }
                #endregion
            }
            else
            {
                model.Components = ComponentDictionaryInstancesCache.GetOrAdd(componentsPath, path => Serializer.DeserializeFromFile<List<CmsComponentInstance>>(path));
            }
            return View(model.View, model);
        }
        #endregion
    }
}
