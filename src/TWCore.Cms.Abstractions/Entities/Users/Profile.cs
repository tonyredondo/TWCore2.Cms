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

using System.Runtime.Serialization;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Entities.Users
{
    /// <summary>
    /// Profile 
    /// </summary>
    [DataContract]
    public class Profile
    {
        /// <summary>
        /// Firstname
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Lastname
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Picture path
        /// </summary>
        [DataMember]
        public string PicturePath { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        [DataMember]
        public string Username { get; set; }
    }
}
