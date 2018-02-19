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
using System.Linq;
using System.Threading.Tasks;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms.Dal
{
    /// <inheritdoc />
    /// <summary>
    /// Dal Base
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    public abstract class DalBase<T> : IDal<T>
    {
        /// <inheritdoc />
        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns>IEnumerable instance with all items</returns>
        public abstract Task<IEnumerable<T>> GetAllAsync();
        /// <inheritdoc />
        /// <summary>
        /// Gets an item by Id
        /// </summary>
        /// <param name="id">Id value</param>
        /// <returns>The item instance</returns>
        public abstract Task<T> GetByIdAsync(string id);
        /// <inheritdoc />
        /// <summary>
        /// Gets an item by Key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>The item instance</returns>
        public abstract Task<T> GetByKeyAsync(string key);
        /// <inheritdoc />
        /// <summary>
        /// Saves an item
        /// </summary>
        /// <param name="value">Item value</param>
        /// <returns>Save task</returns>
        public abstract Task SaveAsync(T value);
        /// <inheritdoc />
        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>The IEnumerable instance with all objects</returns>
        public async Task<IEnumerable<object>> GetAllObjectsAsync() 
            => (await GetAllAsync().ConfigureAwait(false)).Cast<object>();
        /// <inheritdoc />
        /// <summary>
        /// Gets an object by Id
        /// </summary>
        /// <param name="id">Id value</param>
        /// <returns>The object instance</returns>
        public async Task<object> GetObjectByIdAsync(string id) 
            => await GetByIdAsync(id).ConfigureAwait(false);
        /// <inheritdoc />
        /// <summary>
        /// Gets an object by key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>The object instance</returns>
        public async Task<object> GetObjectByKeyAsync(string key) 
            => await GetByKeyAsync(key).ConfigureAwait(false);
        /// <inheritdoc />
        /// <summary>
        /// Saves an object
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Save task</returns>
        public Task SaveAsync(object value) 
            => SaveAsync((T)value);
    }
}
