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
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TWCore.Cms.Web.Components
{
    /// <inheritdoc />
    /// <summary>
    /// Raw Component Result
    /// </summary>
    public class RawComponentResult : IViewComponentResult
    {
        private readonly string _raw;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RawComponentResult(string raw) 
            => _raw = raw;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Execute(ViewComponentContext context)
            => context.Writer.Write(_raw);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task ExecuteAsync(ViewComponentContext context)
            => context.Writer.WriteAsync(_raw);
    }
}
