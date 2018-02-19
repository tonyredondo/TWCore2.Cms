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

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
// ReSharper disable CheckNamespace

namespace TWCore.Cms.Models
{
    /// <summary>
    /// Message Model
    /// </summary>
    [DataContract]
    public class MessageModel
    {
        /// <summary>
        /// Message Type
        /// </summary>
        [DataMember]
        public MessageType Type { get; set; }
        /// <summary>
        /// Message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        #region .ctor
        /// <summary>
        /// Message Model
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MessageModel() { }
        /// <summary>
        /// Message Model
        /// </summary>
        /// <param name="type">Message type</param>
        /// <param name="message">Message value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MessageModel(MessageType type, string message)
        {
            Type = type;
            Message = message;
        }
        #endregion
    }
}