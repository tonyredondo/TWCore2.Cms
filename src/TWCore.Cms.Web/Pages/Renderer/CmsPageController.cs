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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TWCore.Cms.Models.Renderer;
using TWCore.Collections;
using static TWCore.Singleton;

namespace TWCore.Cms.Web.Pages.Renderer
{
    /// <inheritdoc />
    /// <summary>
    /// Cms Page Controller
    /// </summary>
    public class CmsPageController : Controller
    {
        private static readonly TimeoutDictionary<string, bool> ViewExist = new TimeoutDictionary<string, bool>();
        private static readonly string PageConstructText = System.IO.File.ReadAllText("Views/_PageConstruct.cshtml");
        private readonly ICmsRequestData _requestData;
        private readonly ICmsResponseData _responseData;

        #region .ctor
        /// <summary>
        /// Cms Page Controller
        /// </summary>
        /// <param name="requestData">Cms Request Data</param>
        /// <param name="responseData">Cms Response Data</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CmsPageController(ICmsRequestData requestData, ICmsResponseData responseData)
        {
            _requestData = requestData;
            _responseData = responseData;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Action Index
        /// </summary>
        /// <returns>Action Result instance</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async Task<IActionResult> Index()
        {
            var request = HttpContext.Request;
            var global = InstanceOf<Global>();
            var page = global.Business.GetRuntimePageModel(request.Scheme, request.Host.Host, request.Host.Port ?? (request.IsHttps ? 443 : 80), request.Path);
            if (page == null) return Content("Page not found");

            var pageModel = page.Page.GetModel();
            var viewPath = global.GetView(pageModel.ViewPath);

            _requestData.Context = HttpContext;
            _requestData.CreateView = false;
            _requestData.Page = pageModel;
            _requestData.Route = page.Route;
            _responseData.PageResult = View(viewPath, pageModel);

            if (pageModel.BehaviorPipeline?.Any() == true)
            {
                foreach (var bObj in pageModel.BehaviorPipeline)
                {
                    var bType = Core.GetType(bObj);
                    if (bType == null) continue;
                    var processor = (IPageBehavior)Activator.CreateInstance(bType);
                    await processor.InvokeAsync(_requestData, _responseData).ConfigureAwait(false);
                }
            }

            if (ViewExist.GetOrAdd(viewPath, mPath => (System.IO.File.Exists(mPath), TimeSpan.FromSeconds(5))))
                return _responseData.PageResult;

            _requestData.CreateView = true;
            var path = Path.GetDirectoryName(viewPath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var content = PageConstructText;
            lock (page)
                System.IO.File.WriteAllText(viewPath, content);
            ViewExist.TryUpdate(viewPath, true, false);
            return _responseData.PageResult;
        }
        #endregion
    }
}
