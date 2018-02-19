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

namespace TWCore.Cms.Dal.Cache
{
    /// <inheritdoc />
    /// <summary>
    /// Cache Dal Cms
    /// </summary>
    public class CacheDalCms : IDalCms
    {
        #region Properties
        /// <inheritdoc />
        /// <summary>
        /// Scripts DAL
        /// </summary>
        public IDalScript Scripts { get; }
        /// <inheritdoc />
        /// <summary>
        /// Styles DAL
        /// </summary>
        public IDalStylesheet Styles { get; }
        /// <inheritdoc />
        /// <summary>
        /// PagesGroups DAL
        /// </summary>
        public IDalPagesGroup PagesGroups { get; }
        /// <inheritdoc />
        /// <summary>
        /// Sites DAL
        /// </summary>
        public IDalSite Sites { get; }
        /// <inheritdoc />
        /// <summary>
        /// Pages DAL
        /// </summary>
        public IDalPage Pages { get; }
        /// <inheritdoc />
        /// <summary>
        /// Markets DAL
        /// </summary>
        public IDalMarket Markets { get; }
        /// <inheritdoc />
        /// <summary>
        /// Cultures DAL
        /// </summary>
        public IDalCulture Cultures { get; }
        /// <inheritdoc />
        /// <summary>
        /// Users DAL
        /// </summary>
        public IDalUser Users { get; }
        /// <inheritdoc />
        /// <summary>
        /// Components DAL
        /// </summary>
        public IDalComponent Components { get; }
        #endregion

        #region .ctor
        /// <inheritdoc />
        /// <summary>
        /// Cache Dal Cms
        /// </summary>
        public CacheDalCms() : this(false)
        {
        }
        /// <summary>
        /// Cache Dal Cms
        /// </summary>
        /// <param name="useMemory">Use only memory</param>
        public CacheDalCms(bool useMemory)
        {
            if (useMemory)
            {
                Scripts = new CacheDalScript(null);
                Styles = new CacheDalStylesheet(null);
                PagesGroups = new CacheDalPagesGroup(null);
                Sites = new CacheDalSite(null);
                Pages = new CacheDalPage(null);
                Markets = new CacheDalMarket(null);
                Cultures = new CacheDalCulture(null);
                Users = new CacheDalUser(null);
                Components = new CacheDalComponent(null);
            }
            else
            {
                Scripts = new CacheDalScript(Core.Injector.New<IDalScript>());
                Styles = new CacheDalStylesheet(Core.Injector.New<IDalStylesheet>());
                PagesGroups = new CacheDalPagesGroup(Core.Injector.New<IDalPagesGroup>());
                Sites = new CacheDalSite(Core.Injector.New<IDalSite>());
                Pages = new CacheDalPage(Core.Injector.New<IDalPage>());
                Markets = new CacheDalMarket(Core.Injector.New<IDalMarket>());
                Cultures = new CacheDalCulture(Core.Injector.New<IDalCulture>());
                Users = new CacheDalUser(Core.Injector.New<IDalUser>());
                Components = new CacheDalComponent(Core.Injector.New<IDalComponent>());
            }
        }
        #endregion
    }
}
