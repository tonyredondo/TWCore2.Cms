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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;
using TWCore.Cms.Entities.Common;

namespace TWCore.Cms.Dal.Cache
{
    /// <inheritdoc />
    /// <summary>
    /// Cache Dal
    /// </summary>
    /// <typeparam name="T">Type of Dal</typeparam>
    public class CacheDal<T> : DalBase<T> where T : AbstractBasicEntity
    {
        private readonly AsyncLock _lock = new AsyncLock();

        protected IDal<T> Dal;
        protected List<T> Data = new List<T>();
        protected ConcurrentDictionary<string, int> CacheIds = new ConcurrentDictionary<string, int>();
        protected ConcurrentDictionary<string, int> CacheKeys = new ConcurrentDictionary<string, int>();

        #region .ctor
        /// <summary>
        /// Cache Dal
        /// </summary>
        /// <param name="dal">Internal Dal</param>
        public CacheDal(IDal<T> dal)
            => Dal = dal;
        #endregion

        #region Public Methods
        /// <summary>
        /// Get All 
        /// </summary>
        /// <returns>IEnumerable of items</returns>
        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            using (await _lock.LockAsync().ConfigureAwait(false))
            {
                if (Dal == null || Data.Count > 0) return Data.ToArray();
                var data = await Dal.GetAllAsync().ConfigureAwait(false);
                Data = new List<T>(data);
                CacheIds.Clear();
                CacheKeys.Clear();
                return Data.ToArray();
            }
        }
        /// <inheritdoc />
        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id">Id value</param>
        /// <returns>Item</returns>
        public override async Task<T> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var idx = await CacheIds.GetOrAddAsync(id, async kId =>
            {
                var all = await GetAllAsync().ConfigureAwait(false);
                return all.IndexOf(i => i.Id == kId);
            }).ConfigureAwait(false);
            if (idx < 0) return null;
            using (await _lock.LockAsync().ConfigureAwait(false))
                return Data[idx];
        }
        /// <inheritdoc />
        /// <summary>
        /// Get by Key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>Item</returns>
        public override async Task<T> GetByKeyAsync(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            var idx = await CacheKeys.GetOrAddAsync(key, async mKey =>
            {
                var all = await GetAllAsync().ConfigureAwait(false);
                return all.IndexOf(i => i.Key == mKey);
            }).ConfigureAwait(false);
            if (idx < 0) return null;
            using (await _lock.LockAsync().ConfigureAwait(false))
                return Data[idx];
        }
        /// <inheritdoc />
        /// <summary>
        /// Save item
        /// </summary>
        /// <param name="value">Item</param>
        /// <returns>Task</returns>
        public override Task SaveAsync(T value)
            => SaveAsync(value, true);
        /// <summary>
        /// Save item
        /// </summary>
        /// <param name="value">Item</param>
        /// <param name="saveOnInnerDal">Save on inner value</param>
        /// <returns>Task</returns>
        public async Task SaveAsync(T value, bool saveOnInnerDal)
        {
            if (value == null) return;
            using (await _lock.LockAsync().ConfigureAwait(false))
            {
                if (CacheKeys.TryGetValue(value.Key, out var idx))
                    Data[idx] = value;
                else if (value.Id != null && CacheIds.TryGetValue(value.Id, out var idxId))
                    Data[idxId] = value;
                else
                {
                    Data.Add(value);
                    var i = Data.Count - 1;
                    if (value.Id != null)
                        CacheIds.TryAdd(value.Id, i);
                    CacheKeys.TryAdd(value.Key, i);
                }
                if (saveOnInnerDal && Dal != null)
                    await Dal.SaveAsync(value).ConfigureAwait(false);
            }
        }
        #endregion
    }
}
