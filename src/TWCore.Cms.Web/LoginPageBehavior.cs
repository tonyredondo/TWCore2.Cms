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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TWCore.Cms.Models.Renderer;

namespace TWCore.Cms.Web
{
    /// <inheritdoc />
    /// <summary>
    /// Login Page Behavior
    /// </summary>
    public class LoginPageBehavior : IPageBehavior
    {
        public Task InvokeAsync(ICmsRequestData request, ICmsResponseData response)
        {
            request.Page.Data["Value"] = "Login Page Behavior Data";

            if (request.Context.Request.Query["redirect"] == "1")
                response.PageResult = new RedirectResult("http://google.com", false);

            return Task.CompletedTask;
        }
    }
}
