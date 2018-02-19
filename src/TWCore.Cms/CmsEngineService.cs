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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TWCore.Cms.Bll;
using TWCore.Cms.Dal.Cache;
using TWCore.Cms.Entities;
using TWCore.Cms.Entities.Pages;
using TWCore.Cms.Entities.Users;
using TWCore.Net.HttpServer;
using TWCore.Serialization;
using TWCore.Services;

namespace TWCore.Cms
{
    internal class CmsEngineService : TaskService
    {
        public CmsEngineService() : base(token =>
        {
            //***********************************************************************************
            var components = CreateComponents();
            var markets = CreateMarkets();
            var cultures = CreateCultures();
            var pagesGroups = CreatePagesGroups();
            var styles = CreateStyles();
            var scripts = CreateScripts();
            var users = CreateUsers();
            var sites = CreateSites();
            var pages = CreatePages();
            //***********************************************************************************
            JsonTextSerializerExtensions.Serializer.UseCamelCase = false;
            JsonTextSerializerExtensions.Serializer.Indent = true;
            JsonTextSerializerExtensions.Serializer.IgnoreNullValues = true;

            var dalCms = new CacheDalCms(true);
            var bllCms = new BllCms(dalCms);
            Task.WhenAll(components.Select(i => dalCms.Components.SaveAsync(i))).WaitAsync();
            Task.WhenAll(markets.Select(i => dalCms.Markets.SaveAsync(i))).WaitAsync();
            Task.WhenAll(cultures.Select(i => dalCms.Cultures.SaveAsync(i))).WaitAsync();
            Task.WhenAll(styles.Select(i => dalCms.Styles.SaveAsync(i))).WaitAsync();
            Task.WhenAll(scripts.Select(i => dalCms.Scripts.SaveAsync(i))).WaitAsync();
            Task.WhenAll(users.Select(i => dalCms.Users.SaveAsync(i))).WaitAsync();
            Task.WhenAll(sites.Select(i => dalCms.Sites.SaveAsync(i))).WaitAsync();
            Task.WhenAll(pagesGroups.Select(i => dalCms.PagesGroups.SaveAsync(i))).WaitAsync();
            Task.WhenAll(pages.Select(i => dalCms.Pages.SaveAsync(i))).WaitAsync();
            //***********************************************************************************

            #region Create Container

            var container = new CmsContainer();
            container.Components.AddRange(components);
            container.Markets.AddRange(markets);
            container.Cultures.AddRange(cultures);
            container.PagesGroups.AddRange(pagesGroups);
            container.Stylesheets.AddRange(styles);
            container.Scripts.AddRange(scripts);
            container.Users.AddRange(users);
            container.Sites.AddRange(sites);
            container.Pages.AddRange(pages);

            var json = container.SerializeToJson();
            var jsonBytes = Encoding.UTF8.GetBytes(json);
            Core.Log.InfoBasic("JSON container: {0} bytes", jsonBytes.Length);
            container.SerializeToJsonFile("tmp");

            var pwBytes = container.SerializeToWBinary();
            Core.Log.InfoBasic("PWSerializer container: {0}", pwBytes.Count);

            #endregion Create Container

            //***********************************************************************************

            Core.Log.InfoBasic("GetPageModel");
            var pageModel = bllCms.GetRuntimePageModel("http://localhost:49268/");

            //Core.Log.InfoBasic("Render Model");
            //bllCms.Render(pageModel, Console.Out);

            var pModelJson = pageModel.SerializeToJson();
            var pModelJsonBin = Encoding.UTF8.GetBytes(pModelJson);
            Core.Log.InfoBasic("Page Model Json: {0} bytes", pModelJsonBin.Length);

            var pModelBin = pageModel.SerializeToWBinary();
            Core.Log.InfoBasic("Page Model: {0} bytes", pModelBin.Count);
            pageModel.SerializeToWBinaryFile("pageModel");

            pageModel.SerializeToJsonFile("pagemodel.json");

            var shs = new SimpleHttpServer();
            shs.OnBeginRequest += (HttpContext context, ref bool handled, CancellationToken cancellationToken) =>
            {
                var url = context.Request.Url.OriginalString;
                var model = bllCms.GetRuntimePageModel(url);
                if (model != null)
                {
                    //using (var sw = new StreamWriter(context.Response.OutputStream, Encoding.UTF8, 4096, true))
                    //    bllCms.Render(model, sw);
                    handled = true;
                }
            };
            shs.StartAsync(8086);

        })
        {
            //EndAfterTaskFinish = true;
        }

        private static Page[] CreatePages()
        {
            return new[]
            {
                new Page
                {
                    Id = "pages/1",
                    Key = "HomePage",
                    Name = "HomePage",
                    Description = "Home page",
                    Notes = "Notes",
                    Rev = 1,
                    RevComment = "Version inicial",
                    BehaviorPipelineTypes = new List<string>
                    {
                        "MySite.Login, MySite",
                        "MySite.Pages.HomePage, MySite"
                    },
                    DateFrom = DateTime.MinValue,
                    DateTo = DateTime.MaxValue,
                    Robots = RobotsTag.IndexFollow,
                    IncludeInSiteMap = true,
                    Instances = new PageInstanceCollection
                    {
                        new PageInstance
                        {
                            MarketKey = "fr",
                            CultureKey = "fr",
                            Routes = new List<string> { "/", "/Home" },
                            Title = "Page d'accueil",
                            Keywords = "Accueil, Page",
                            ForwardMode = RedirectCode.Permanently,
                            ForwardUrl = "/404/fr",
                            PublishStatus = PublishStatus.Published,
                            ParentComponent = new ComponentInstance
                            {
                                ComponentKey = "BasicLayoutComponent",
                                Children = new List<ComponentInstance>
                                {
                                    new ComponentInstance
                                    {
                                        ComponentKey = "H1Component", LayoutOrder = 0, PlaceholderZone = "Main", Vars = new Variables
                                        {
                                            Data = new ValueDictionary { ["Value"] = "Ceci est ma page d'accueil" }
                                        }
                                    }
                                }
                            },
                            Vars = new Variables
                            {
                                Scripts = new ValueCollection { "page.fr" },
                                Styles = new ValueCollection { "page.fr" },
                            },
                        },
                        new PageInstance
                        {
                            MarketKey = "es",
                            CultureKey = "es",
                            Routes = new List<string> { "/", "/Inicio", "/Name/{name}" },
                            Title = "Página de Inicio",
                            Keywords = "Página de Inicio",
                            ForwardMode = RedirectCode.Permanently,
                            ForwardUrl = "/404/es",
                            PublishStatus = PublishStatus.Published,
                            ParentComponent = new ComponentInstance
                            {
                                ComponentKey = "BasicLayoutComponent",
                                Children = new List<ComponentInstance>
                                {
                                    new ComponentInstance
                                    {
                                        ComponentKey = "H1Component", LayoutOrder = 0, PlaceholderZone = "Main", Vars = new Variables
                                        {
                                            Data = new ValueDictionary { ["Value"] = "Esta es mi página de inicio" }
                                        }
                                    }
                                },
                            },
                            Vars = new Variables
                            {
                                Scripts = new ValueCollection { "page.es" },
                                Styles = new ValueCollection { "page.es" },
                            },
                        }
                    },
                    Enabled = true,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now
                }
            };
        }

        private static Site[] CreateSites()
        {
            return new[]
            {
                new Site
                {
                    Id = "sites/1",
                    Key = "SampleSite",
                    Rev = 1,
                    RevComment = "Version inicial",
                    Name = "Test Site",
                    Description = "This is a Test Site",
                    UrlBindings = new List<UrlBinding> { "http://localhost:49268", "http://10.10.0.45:5102/", "http://localhost:5102/", "http://*:0/" },
                    MarketKey = "es",
                    CultureKey = "es",
                    PagesGroups = new List<string> { "WhiteLabel" },
                    Header = new ComponentInstance
                    {
                        ComponentKey = "H1Component",
                        Vars = new Variables
                        {
                            Data = new ValueDictionary { ["Values"] = "Header: Mi primer H1" }
                        }
                    },
                    Footer = new ComponentInstance
                    {
                        ComponentKey = "H2Component",
                        Vars = new Variables
                        {
                            Data = new ValueDictionary { ["Values"] = "Footer: Mi primer H2" }
                        }
                    },
                    Vars = new Variables
                    {
                        Scripts = new ValueCollection { "site" },
                        Styles = new ValueCollection { "site" },
                        Data = new ValueDictionary { ["Option1"] = true }
                    },
                    Published = true,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Site
                {
                    Id = "sites/2",
                    Key = "SampleInnerSite",
                    Name = "Test Inner Site",
                    Description = "This is a Test for a Inner Site",
                    UrlBindings = new List<UrlBinding> { "http://localhost:49268/inner/", "http://10.10.0.45:5102/inner/", "http://localhost:5102/inner/", "http://*:0/inner/" },
                    MarketKey = "fr",
                    CultureKey = "fr",
                    PagesGroups = new List<string> { "B2C" },
                    Header = new ComponentInstance
                    {
                        ComponentKey = "H1Component",
                        Vars = new Variables
                        {
                            Data = new ValueDictionary { ["Values"] = "Header: Mi primer H1 en un inner site" }
                        }
                    },
                    Footer = new ComponentInstance
                    {
                        ComponentKey = "H2Component",
                        Vars = new Variables
                        {
                            Data = new ValueDictionary { ["Values"] = "Footer: Mi primer H2 en un inner site" }
                        }
                    },
                    Vars = new Variables
                    {
                        Scripts = new ValueCollection { "site" },
                        Styles = new ValueCollection { "site" },
                        Data = new ValueDictionary { ["Option1"] = true }
                    },
                    Published = true,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                }
            };
        }

        private static User[] CreateUsers()
        {
            return new[]
            {
                new User
                {
                    Id = "users/1",
                    Key = "tony",
                    Profile = new Profile
                    {
                        FirstName = "Daniel",
                        LastName = "Redondo",
                        Email = "tonyredondo@gmail.com",
                        PicturePath = "profilepic.png"
                    },
                    Sites = new Dictionary<string, SiteRoles>
                    {
                        ["SampleSite"] = SiteRoles.Read | SiteRoles.Write | SiteRoles.Publish
                    },
                    Username = "tony",
                    Password = "123",
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new User
                {
                    Id = "users/2",
                    Key = "gabo",
                    Profile = new Profile
                    {
                        FirstName = "Gabriel",
                        LastName = "Redondo",
                        Email = "redondogabriel@gmail.com",
                        PicturePath = "profilepic.png"
                    },
                    Sites = new Dictionary<string, SiteRoles>
                    {
                        ["SampleSite"] = SiteRoles.Read | SiteRoles.Write
                    },
                    Username = "gabriel",
                    Password = "123",
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new User
                {
                    Id = "users/3",
                    Key = "aura",
                    Profile = new Profile
                    {
                        FirstName = "Aura",
                        LastName = "Maneiro",
                        PicturePath = "profilepic.png"
                    },
                    Sites = new Dictionary<string, SiteRoles>
                    {
                        ["SampleSite"] = SiteRoles.Read | SiteRoles.Write | SiteRoles.Publish
                    },
                    Username = "aura",
                    Password = "123",
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new User
                {
                    Id = "users/5",
                    Key = "guest",
                    Profile = new Profile
                    {
                        FirstName = "Guest",
                        LastName = "",
                        PicturePath = "profilepic.png"
                    },
                    Sites = new Dictionary<string, SiteRoles>
                    {
                        ["SampleSite"] = SiteRoles.Read
                    },
                    Username = "guest",
                    Password = "123",
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                }
            };
        }

        private static PagesGroup[] CreatePagesGroups()
        {
            return new[]
            {
                new PagesGroup
                {
                    Id = "pagesgroups/1",
                    Key = "B2C",
                    Name = "B2C Pages Group",
                    Description = "This is the B2C site type values",
                    LocalizedVars = new LocalizedVariablesCollection
                    {
                        new LocalizedVariables
                        {
                            MarketKey = "*",
                            CultureKey = "*",
                            Vars = new Variables
                            {
                                Scripts = new ValueCollection { "group.b2c" },
                                Styles = new ValueCollection { "group.b2c" },
                            }
                        }
                    },
                    PagesKey = new List<string> { "HomePage" },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new PagesGroup
                {
                    Id = "pagesgroups/2",
                    Key = "WhiteLabel",
                    Name = "White Label",
                    Description = "This is the White Label site type values",
                    LocalizedVars = new LocalizedVariablesCollection
                    {
                        new LocalizedVariables
                        {
                            MarketKey = "*",
                            CultureKey = "*",
                            Vars = new Variables
                            {
                                Scripts = new ValueCollection { "group.wl" },
                                Styles = new ValueCollection { "group.wl" },
                            }
                        }
                    },
                    PagesKey = new List<string> { "HomePage" },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                }
            };
        }

        private static Component[] CreateComponents()
        {
            return new[]
            {
                new Component
                {
                    Id = "components/1",
                    Key = "BasicLayoutComponent",
                    Name = "Basic Layout Component",
                    Description = "Basic Layout Component",
                    PlaceholderType = ComponentPlaceholderType.Layout,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            ViewContent = "<header>[[HEADER]]</header><main>[[CONTENT:Main]]</main><footer>[[FOOTER]]</footer>",
                            ViewContentType = ComponentContentType.Html
                        }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Component
                {
                    Id = "components/10",
                    Key = "H1Component",
                    Name = "H1 Component",
                    Description = "Basic H1 component",
                    PlaceholderType = ComponentPlaceholderType.Content | ComponentPlaceholderType.Header | ComponentPlaceholderType.Footer,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            CultureKey = "*",
                            MarketKey = "*",
                            ViewContent = "<h1>@Model.Data[\"Value\"]</h1>",
                            ViewContentType = ComponentContentType.Html,
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "H1" }
                            }
                        }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Component
                {
                    Id = "components/11",
                    Key = "H2Component",
                    Name = "H2 Component",
                    Description = "Basic H2 component",
                    PlaceholderType = ComponentPlaceholderType.Content | ComponentPlaceholderType.Header | ComponentPlaceholderType.Footer,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            CultureKey = "*",
                            MarketKey = "*",
                            ViewContent = "<h2>@Model.Data[\"Value\"]</h2>",
                            ViewContentType = ComponentContentType.Html,
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "H2" }
                            }
                        }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Component
                {
                    Id = "components/12",
                    Key = "H3Component",
                    Name = "H3 Component",
                    Description = "Basic H3 component",
                    PlaceholderType = ComponentPlaceholderType.Content | ComponentPlaceholderType.Header | ComponentPlaceholderType.Footer,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            CultureKey = "*",
                            MarketKey = "*",
                            ViewContent = "<h3>@Model.Data[\"Value\"]</h3>",
                            ViewContentType = ComponentContentType.Html,
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "H3" }
                            }
                        }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Component
                {
                    Id = "components/13",
                    Key = "PComponent",
                    Name = "Paragraph Component",
                    Description = "Basic paragraph component",
                    PlaceholderType = ComponentPlaceholderType.Content | ComponentPlaceholderType.Header | ComponentPlaceholderType.Footer,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            CultureKey = "*",
                            MarketKey = "*",
                            ViewContent = "<p>@Model.Data[\"Value\"]</p>",
                            ViewContentType = ComponentContentType.Html,
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "H4" }
                            }
                        }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Component
                {
                    Id = "components/14",
                    Key = "IMGComponent",
                    Name = "Image Component",
                    Description = "Basic image component",
                    PlaceholderType = ComponentPlaceholderType.Content | ComponentPlaceholderType.Header | ComponentPlaceholderType.Footer,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            CultureKey = "*",
                            MarketKey = "*",
                            ViewContent = "<img " +
                                "src=\"@(Model.Data[\"Src\"])\" " +
                                "height=\"@(Model.Data[\"Height\"])\" " +
                                "width=\"@(Model.Data[\"Width\"])\" " +
                                "srcset=\"@(Model.Data[\"SrcSet\"])\" " +
                                "sizes=\"@(Model.Data[\"Sizes\"])\" " +
                                "alt=\"@(Model.Data[\"Alt\"])\" " +
                                "usemap=\"@(Model.Data[\"Usemap\"])\" " +
                                "/>",
                            ViewContentType = ComponentContentType.Html,
                            Vars = new Variables
                            {
                                Data = new ValueDictionary
                                {
                                    ["Src"] = "/content/empty.jpg",
                                    ["Height"] = "25px",
                                    ["Width"] = "25px",
                                    ["SrcSet"] = (string)null,
                                    ["Sizes"] = (string)null,
                                    ["Alt"] = null,
                                    ["Usemap"] = null,
                                },
                            }
                        }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Component
                {
                    Id = "components/20",
                    Key = "HtmlComponent",
                    Name = "Html Component",
                    Description = "Basic HTML component",
                    PlaceholderType = ComponentPlaceholderType.Content,
                    Locales = new ComponentLocaleCollection
                    {
                        new ComponentLocale
                        {
                            MarketKey = "*",
                            CultureKey = "fr",
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "Ici devrait être le contenu Html" }
                            },
                            ViewContent = "@Html.Raw(Model.Data[\"Value\"])",
                            ViewContentType = ComponentContentType.Html,
                            EditorContent = "And Here should be the Editor",
                            EditorContentType = ComponentContentType.Html,
                        },
                        new ComponentLocale
                        {
                            MarketKey = "*",
                            CultureKey = "es",
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "Aqui deberia estar el contenido Html" }
                            },
                            ViewContent = "@Html.Raw(Model.Data[\"Value\"])",
                            ViewContentType = ComponentContentType.Html,
                            EditorContent = "And Here should be the Editor",
                            EditorContentType = ComponentContentType.Html,
                        },
                        new ComponentLocale
                        {
                            MarketKey = "*",
                            CultureKey = "*",
                            Vars = new Variables
                            {
                                Data = new ValueDictionary { ["Value"] = "Here should be the Html Content" }
                            },
                            ViewContent = "@Html.Raw(Model.Data[\"Value\"])",
                            ViewContentType = ComponentContentType.Html,
                            EditorContent = "And Here should be the Editor",
                            EditorContentType = ComponentContentType.Html,
                        },
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                }
            };
        }

        private static Culture[] CreateCultures()
        {
            return new[]
            {
                new Culture
                {
                    Id = "cultures/1",
                    Key = "fr",
                    Name = "French",
                    IsoTag = "fr",
                    Description = "French Culture",
                    Notes = "French Culture",
                    PicturePath = "fr-flag.png",
                    FirstDayOfWeek = DayOfWeek.Monday,
                    ShortDateFormat = "dd/MM/yyyy",
                    LongDateFormat = "ddd, dd MMM yyyy",
                    ShortTimeFormat = "Hhmm",
                    LongTimeFormat = "H:mm:ss",
                    Vars = new Variables
                    {
                        Styles = new ValueCollection { "culture.fr" },
                        Scripts = new ValueCollection { "culture.fr" },
                        Data = new ValueDictionary { }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Culture
                {
                    Id = "cultures/2",
                    Key = "es",
                    Name = "Spanish",
                    IsoTag = "es",
                    Description = "Spanish Culture",
                    Notes = "Spanish Culture",
                    PicturePath = "es-flag.png",
                    FirstDayOfWeek = DayOfWeek.Monday,
                    ShortDateFormat = "dd/MM/yyyy",
                    LongDateFormat = "dddd, d' de 'MMMM' de 'aaaa",
                    ShortTimeFormat = "H:mm",
                    LongTimeFormat = "H:mm:ss",
                    Vars = new Variables
                    {
                        Styles = new ValueCollection { "culture.es" },
                        Scripts = new ValueCollection { "culture.es" },
                        Data = new ValueDictionary { }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                }
            };
        }

        private static Market[] CreateMarkets()
        {
            return new[]
            {
                new Market
                {
                    Id = "markets/1",
                    Key = "fr",
                    Name = "French",
                    IsoTag = "fr",
                    Description = "French Market",
                    Notes = string.Empty,
                    PicturePath = "fr-flag.png",
                    Vars = new Variables
                    {
                        Scripts = new ValueCollection { "market.fr" },
                        Styles = new ValueCollection { "market.fr" },
                        Data = new ValueDictionary { }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Market
                {
                    Id = "markets/2",
                    Key = "es",
                    Name = "Spanish",
                    IsoTag = "es",
                    Description = "Spanish Market",
                    Notes = string.Empty,
                    PicturePath = "es-flag.png",
                    Vars = new Variables
                    {
                        Scripts = new ValueCollection { "market.es" },
                        Styles = new ValueCollection { "market.es" },
                        Data = new ValueDictionary { }
                    },
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                }
            };
        }

        private static Script[] CreateScripts()
        {
            return new[]
            {
                new Script
                {
                    Id = "scripts/1",
                    Key = "market.fr",
                    Name = "market.fr",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('French Market')",
                    CollectionType = CollectionType.Market,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/2",
                    Key = "market.es",
                    Name = "market.es",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('Spanish Market')",
                    CollectionType = CollectionType.Market,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/3",
                    Key = "culture.fr",
                    Name = "culture.fr",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('French Culture')",
                    CollectionType = CollectionType.Culture,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/4",
                    Key = "culture.es",
                    Name = "culture.es",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('Spanish Culture')",
                    CollectionType = CollectionType.Culture,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/5",
                    Key = "group.wl",
                    Name = "group.wl",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('White Label Pages Group')",
                    CollectionType = CollectionType.PagesGroup,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/6",
                    Key = "group.b2c",
                    Name = "group.b2c",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('B2C Pages Group')",
                    CollectionType = CollectionType.PagesGroup,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/7",
                    Key = "site",
                    Name = "site",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('Sample Site')",
                    CollectionType = CollectionType.Site,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/8",
                    Key = "page.fr",
                    Name = "page.fr",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('Page JS')",
                    CollectionType = CollectionType.Page,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Script
                {
                    Id = "scripts/9",
                    Key = "page.es",
                    Name = "page.es",
                    Type = ScriptType.InlineJs,
                    Source = "window.alert('Page JS')",
                    CollectionType = CollectionType.Page,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
            };
        }

        private static Stylesheet[] CreateStyles()
        {
            return new[]
            {
                new Stylesheet
                {
                    Id = "stylesheets/1",
                    Key = "market.fr",
                    Name = "market.fr",
                    Type = StylesheetType.Css,
                    Source = "market.fr.css",
                    CollectionType = CollectionType.Market,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/2",
                    Key = "market.es",
                    Name = "market.es",
                    Type = StylesheetType.Css,
                    Source = "market.es.css",
                    CollectionType = CollectionType.Market,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/3",
                    Key = "culture.fr",
                    Name = "culture.fr",
                    Type = StylesheetType.Css,
                    Source = "culture.fr.css",
                    CollectionType = CollectionType.Culture,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/4",
                    Key = "culture.es",
                    Name = "culture.es",
                    Type = StylesheetType.Css,
                    Source = "culture.es.css",
                    CollectionType = CollectionType.Culture,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/5",
                    Key = "group.wl",
                    Name = "group.wl",
                    Type = StylesheetType.Css,
                    Source = "group.wl.css",
                    CollectionType = CollectionType.PagesGroup,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/6",
                    Key = "group.b2c",
                    Name = "group.b2c",
                    Type = StylesheetType.Css,
                    Source = "group.b2c.css",
                    CollectionType = CollectionType.PagesGroup,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/7",
                    Key = "site",
                    Name = "site",
                    Type = StylesheetType.Css,
                    Source = "site.css",
                    CollectionType = CollectionType.Site,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/8",
                    Key = "page.fr",
                    Name = "page.fr",
                    Type = StylesheetType.Css,
                    Source = "page.fr.css",
                    CollectionType = CollectionType.Page,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
                new Stylesheet
                {
                    Id = "stylesheets/9",
                    Key = "page.es",
                    Name = "page.es",
                    Type = StylesheetType.Css,
                    Source = "page.es.css",
                    CollectionType = CollectionType.Page,
                    CreationDate = Core.Now,
                    UpdateDate = Core.Now,
                    Enabled = true
                },
            };
        }

    }
}