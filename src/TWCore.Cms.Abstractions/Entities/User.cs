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
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using TWCore.Cms.Entities.Common;
using TWCore.Cms.Entities.Users;
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms.Entities
{
	/// <inheritdoc />
	/// <summary>
	/// User definition
	/// </summary>
	[DataContract]
	public class User : AbstractBasicEntity
	{
		/// <summary>
		/// User profile
		/// </summary>
		[DataMember]
		public Profile Profile { get; set; }
		/// <summary>
		/// Sites roles
		/// </summary>
		[DataMember]
		public Dictionary<string, SiteRoles> Sites { get; set; } = new Dictionary<string, SiteRoles>();
		/// <summary>
		/// Username
		/// </summary>
		[DataMember]
		public string Username 
		{ 
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Profile.Username;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Profile.Username = value;
		}
		/// <summary>
		/// Password
		/// </summary>
		[DataMember]
		public string Password { get; set; }
	}
}
