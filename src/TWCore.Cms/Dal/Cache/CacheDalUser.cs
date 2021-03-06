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

using TWCore.Cms.Entities;

namespace TWCore.Cms.Dal.Cache
{
    /// <summary>
    /// User Cache Dal
    /// </summary>
    public class CacheDalUser : CacheDal<User>, IDalUser
    {
        /// <inheritdoc />
        /// <summary>
        /// User Cache Dal
        /// </summary>
        /// <param name="dal">Original IDal</param>
        public CacheDalUser(IDalUser dal) : base(dal) { }
    }
}
