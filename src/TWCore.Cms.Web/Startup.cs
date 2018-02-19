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

using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TWCore.Cms.Models.Renderer;
using TWCore.Serialization;
using TWCore.Serialization.WSerializer;
using TWCore.Web;
using static TWCore.Singleton;

namespace TWCore.Cms.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var global = InstanceOf<Global>();
            global.ViewsLocation = Path.Combine("wwwroot", "cachedViews");

            SerializerManager.DefaultBinarySerializer = new WBinarySerializer();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddResponseCaching();
            services.SetDefaultTWCoreValues();
            services.Configure<RazorViewEngineOptions>(o =>
            {
                //o.ViewLocationExpanders.Add(new CmsLocationExpander());
            });
            services.AddScoped<ICmsRequestData, CmsRequestData>();
            services.AddScoped<ICmsResponseData, CmsResponseData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/500");
            }
            //app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "empty",
                    template: "",
                    defaults: new { controller = "CmsPage", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{*url}",
                    defaults: new { controller = "CmsPage", action = "Index" },
                    constraints: new { url = "^(?!/static/|static/).*" });
            });
        }
    }
}