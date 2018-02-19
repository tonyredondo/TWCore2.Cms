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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TWCore.Cms.Bll;
using TWCore.Cms.Dal;
using TWCore.Cms.Dal.Cache;
using TWCore.Serialization;

namespace TWCore.Cms.Web
{
    /// <summary>
    /// Global
    /// </summary>
    public class Global
    {
        private readonly CmsContainer _container;

        #region Properties
        /// <summary>
        /// Data access layer
        /// </summary>
        public IDalCms Dal { get; private set; }
        /// <summary>
        /// Business instance
        /// </summary>
        public IBllCms Business { get; private set; }
        /// <summary>
        /// Views location
        /// </summary>
        public string ViewsLocation { get; set; } = "wwwwroot/cachedViews";
        /// <summary>
        /// Components location
        /// </summary>
        public string ComponentsLocation { get; set; } = "wwwroot/components";
        #endregion

        #region .ctor
        /// <summary>
        /// Global
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Global()
        {
            SerializerManager.SupressFileExtensionWarning = true;
            if (File.Exists("testData.json"))
            {
                JsonTextSerializerExtensions.Serializer.UseCamelCase = false;
                JsonTextSerializerExtensions.Serializer.Indent = true;
                JsonTextSerializerExtensions.Serializer.IgnoreNullValues = true;
                _container = "testData.json".DeserializeFromJsonFile<CmsContainer>();
            }
            else
                Core.Log.Warning("The testData.json file can't be loaded.");
            _container = _container ?? new CmsContainer();
        }
        #endregion

        #region Private Methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void LoadArgumentKeyValue(string key, string value)
        {
            if (key != "/cpath" || !value.IsNotNullOrWhitespace()) return;

            if (Directory.Exists(value))
            {
                var components = Directory.GetFiles(value, "component.json", SearchOption.AllDirectories);
                foreach (var component in components)
                    Try.Do(() => LoadComponent(component), false);
            }
            else
                Core.Log.Warning("The Component Directory: {0} doesn't exist.", value);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get view path
        /// </summary>
        /// <param name="viewPath">View path</param>
        /// <returns>View path</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetView(string viewPath)
            => Path.Combine(ViewsLocation, viewPath);
        /// <summary>
        /// Load Arguments
        /// </summary>
        /// <param name="args">Arguments</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task LoadArgumentsAsync(string[] args)
        {
            try
            {
                if (args?.Any() == true)
                {
                    for (var i = 0; i < args.Length; i++)
                    {
                        var kv = args[i].Split('=');
                        var key = kv[0].ToLowerInvariant();
                        var value = kv[1];
                        LoadArgumentKeyValue(key, value);
                    }
                }

                await LoadAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Core.Log.Write(ex);
            }
        }

        /// <summary>
        /// Load component
        /// </summary>
        /// <param name="componentFilePath">Component file path</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadComponent(string componentFilePath)
        {
            var cBasePath = Path.GetDirectoryName(componentFilePath);
            var component = componentFilePath.DeserializeFromJsonFile<Entities.Component>();
            if (component == null) return;
            
            Core.Log.InfoBasic("Loading Installed Component: {0} ({1})", component.Name, component.Key);
            if (component.Locales?.Any() == true)
            {
                foreach(var clocale in component.Locales)
                {
                    if (clocale.ViewContentType == ComponentContentType.Url)
                    {
                        var cPath = Path.Combine(cBasePath, clocale.ViewContent);
                        if (File.Exists(cPath))
                        {
                            clocale.ViewContent = File.ReadAllText(cPath);
                            clocale.ViewContentType = ComponentContentType.Html;
                        }
                        else
                        {
                            Core.Log.Warning("The View of the component could'nt be found: {0}", cPath);
                            return;
                        }
                    }
                    if (clocale.EditorContentType == ComponentContentType.Url)
                    {
                        var cPath = Path.Combine(cBasePath, clocale.EditorContent);
                        if (File.Exists(cPath))
                        {
                            clocale.EditorContent = File.ReadAllText(cPath);
                            clocale.EditorContentType = ComponentContentType.Html;
                        }
                        else
                        {
                            Core.Log.Warning("The Editor view of the component could'nt be found: {0}", cPath);
                            return;
                        }
                    }
                }
            }
            _container.Components.RemoveCollection(c => c.KeyWithRev == component.KeyWithRev);
            _container.Components.Add(component);
        }

        /// <summary>
        /// Load Async
        /// </summary>
        /// <returns>Task instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task LoadAsync()
        {
            Dal = new CacheDalCms(true);
            Business = new BllCms(Dal);

            foreach (var item in _container.Components)
                await Dal.Components.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Cultures)
                await Dal.Cultures.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Markets)
                await Dal.Markets.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Pages)
                await Dal.Pages.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.PagesGroups)
                await Dal.PagesGroups.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Scripts)
                await Dal.Scripts.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Sites)
                await Dal.Sites.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Stylesheets)
                await Dal.Styles.SaveAsync(item).ConfigureAwait(false);
            foreach (var item in _container.Users)
                await Dal.Users.SaveAsync(item).ConfigureAwait(false);

            await Business.PrepareSiteRoutesAsync().ConfigureAwait(false);
        }
        #endregion
    }
}
