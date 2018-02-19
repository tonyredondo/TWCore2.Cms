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

using TWCore.Serialization;
using TWCore.Serialization.WSerializer;
using TWCore.Services;
using static TWCore.Singleton;

namespace TWCore.Cms.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Core.InitDefaults(false);
            SerializerManager.DefaultBinarySerializer = new WBinarySerializer();
            Core.RunOnInit(async () =>
            {
                var global = InstanceOf<Global>();
                await global.LoadArgumentsAsync(args).ConfigureAwait(false);
            });
            Core.RunService(() => WebService.Create<Startup>(), args);
        }
    }
}