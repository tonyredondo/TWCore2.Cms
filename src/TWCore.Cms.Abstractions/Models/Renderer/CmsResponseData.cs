﻿/*
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
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models.Renderer
{
    /// <inheritdoc />
    /// <summary>
    /// Cms response data
    /// </summary>
    public class CmsResponseData : ICmsResponseData
    {
        /// <inheritdoc />
        /// <summary>
        /// Page action result
        /// </summary>
        public IActionResult PageResult { get; set; }
    }
}
