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

using System.Collections.Generic;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Entities.Pages
{
    /// <inheritdoc />
    /// <summary>
    /// Page instance collection
    /// </summary>
    [DataContract]
    public class PageInstanceCollection : List<PageInstance>
    {
        /// <summary>
        /// Gets a list of page instance
        /// </summary>
        /// <param name="marketKey">Market key</param>
        /// <param name="cultureKey">Culture key</param>
        /// <param name="pagesGroupKey">Pages group key</param>
        /// <param name="siteKey">Site key</param>
        /// <returns>List of page instance</returns>
        public List<PageInstance> Get(string marketKey, string cultureKey, string pagesGroupKey, string siteKey)
        {
            if (string.IsNullOrWhiteSpace(marketKey))
                marketKey = "*";
            if (string.IsNullOrWhiteSpace(cultureKey))
                cultureKey = "*";
            if (string.IsNullOrWhiteSpace(pagesGroupKey))
                pagesGroupKey = "*";
            if (string.IsNullOrWhiteSpace(siteKey))
                siteKey = "*";

            var res = FindAll(item => item.MarketKey == marketKey && item.CultureKey == cultureKey && item.PagesGroupKey == pagesGroupKey && item.SiteKey == siteKey);
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == marketKey && item.CultureKey == cultureKey && item.PagesGroupKey == pagesGroupKey && item.SiteKey == "*");
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == marketKey && item.CultureKey == cultureKey && item.PagesGroupKey == "*" && item.SiteKey == siteKey);
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == marketKey && item.CultureKey == cultureKey && item.PagesGroupKey == "*" && item.SiteKey == "*");
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == marketKey && item.CultureKey == "*" && item.PagesGroupKey == pagesGroupKey && item.SiteKey == siteKey);
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == "*" && item.CultureKey == cultureKey && item.PagesGroupKey == pagesGroupKey && item.SiteKey == siteKey);
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == "*" && item.CultureKey == "*" && item.PagesGroupKey == pagesGroupKey && item.SiteKey == siteKey);
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == "*" && item.CultureKey == "*" && item.PagesGroupKey == pagesGroupKey && item.SiteKey == "*");
            if (res.Count > 0)
                return res;

            res = FindAll(item => item.MarketKey == "*" && item.CultureKey == "*" && item.PagesGroupKey == "*" && item.SiteKey == "*");
            if (res.Count > 0)
                return res;

            return res;
        }

        /// <summary>
        /// Removes pages instances
        /// </summary>
        /// <param name="marketKey">Market key</param>
        /// <param name="cultureKey">Culture key</param>
        /// <param name="pagesGroupKey">Pages group key</param>
        /// <param name="siteKey">Site key</param>
        /// <returns>List of page instances removed</returns>
        public List<PageInstance> Remove(string marketKey, string cultureKey, string pagesGroupKey, string siteKey)
        {
            var values = Get(marketKey, cultureKey, pagesGroupKey, siteKey);
            if (values.Count > 0)
                values.ForEach(i => Remove(i));
            return values;
        }

        /// <summary>
        /// Removes all pages instances except the selecteds
        /// </summary>
        /// <param name="marketKey">Market key</param>
        /// <param name="cultureKey">Culture key</param>
        /// <param name="pagesGroupKey">Pages group key</param>
        /// <param name="siteKey">Site key</param>
        /// <returns>List of page instances kept</returns>
        public List<PageInstance> RemoveAllBut(string marketKey, string cultureKey, string pagesGroupKey, string siteKey)
        {
            var values = Get(marketKey, cultureKey, pagesGroupKey, siteKey);
            if (values.Count <= 0) return values;
            Clear();
            AddRange(values);
            return values;
        }
    }

}
