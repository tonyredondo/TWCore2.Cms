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
using System.Threading.Tasks;
using TWCore.Cms.Entities;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

namespace TWCore.Cms.Dal
{
    /// <summary>
    /// Basic IDal interface
    /// </summary>
    public interface IDal
    {
        /// <summary>
        /// Get all objects
        /// </summary>
        /// <returns>The IEnumerable instance with all objects</returns>
        Task<IEnumerable<object>> GetAllObjectsAsync();
        /// <summary>
        /// Gets an object by Id
        /// </summary>
        /// <param name="id">Id value</param>
        /// <returns>The object instance</returns>
        Task<object> GetObjectByIdAsync(string id);
        /// <summary>
        /// Gets an object by key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>The object instance</returns>
        Task<object> GetObjectByKeyAsync(string key);
        /// <summary>
        /// Saves an object
        /// </summary>
        /// <param name="value">Object value</param>
        /// <returns>Save task</returns>
        Task SaveAsync(object value);
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Basic IDal generic interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDal<T>: IDal
    {
        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns>IEnumerable instance with all items</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Gets an item by Id
        /// </summary>
        /// <param name="id">Id value</param>
        /// <returns>The item instance</returns>
        Task<T> GetByIdAsync(string id);
        /// <summary>
        /// Gets an item by Key
        /// </summary>
        /// <param name="key">Key value</param>
        /// <returns>The item instance</returns>
        Task<T> GetByKeyAsync(string key);
        /// <summary>
        /// Saves an item
        /// </summary>
        /// <param name="value">Item value</param>
        /// <returns>Save task</returns>
        Task SaveAsync(T value);
    }
    
    /// <summary>
    /// Cms DAL interface
    /// </summary>
    public interface IDalCms
    {
        /// <summary>
        /// Scripts DAL
        /// </summary>
        IDalScript Scripts { get; }
        /// <summary>
        /// Styles DAL
        /// </summary>
        IDalStylesheet Styles { get; }
        /// <summary>
        /// PagesGroups DAL
        /// </summary>
        IDalPagesGroup PagesGroups { get; }
        /// <summary>
        /// Sites DAL
        /// </summary>
        IDalSite Sites { get; }
        /// <summary>
        /// Pages DAL
        /// </summary>
        IDalPage Pages { get; }
        /// <summary>
        /// Markets DAL
        /// </summary>
        IDalMarket Markets { get; }
        /// <summary>
        /// Cultures DAL
        /// </summary>
        IDalCulture Cultures { get; }
        /// <summary>
        /// Users DAL
        /// </summary>
        IDalUser Users { get; }
        /// <summary>
        /// Components DAL
        /// </summary>
        IDalComponent Components { get; }
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Script DAL interface
    /// </summary>
    public interface IDalScript : IDal<Script> { }
    /// <inheritdoc />
    /// <summary>
    /// Stylesheet DAL interface
    /// </summary>
    public interface IDalStylesheet : IDal<Stylesheet> { }
    /// <inheritdoc />
    /// <summary>
    /// PagesGroup DAL interface
    /// </summary>
    public interface IDalPagesGroup : IDal<PagesGroup> { }
    /// <inheritdoc />
    /// <summary>
    /// Component DAL interface
    /// </summary>
    public interface IDalComponent : IDal<Component> { }
    /// <inheritdoc />
    /// <summary>
    /// Site DAL interface
    /// </summary>
    public interface IDalSite : IDal<Site> { }
    /// <inheritdoc />
    /// <summary>
    /// Page DAL interface
    /// </summary>
    public interface IDalPage : IDal<Page> { }
    /// <inheritdoc />
    /// <summary>
    /// Market DAL interface
    /// </summary>
    public interface IDalMarket : IDal<Market> { }
    /// <inheritdoc />
    /// <summary>
    /// Culture DAL interface
    /// </summary>
    public interface IDalCulture : IDal<Culture> { }
    /// <inheritdoc />
    /// <summary>
    /// User DAL interface
    /// </summary>
    public interface IDalUser : IDal<User> { }
}
