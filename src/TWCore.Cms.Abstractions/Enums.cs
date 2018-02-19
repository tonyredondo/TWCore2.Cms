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
// ReSharper disable CheckNamespace

namespace TWCore.Cms
{
    /// <summary>
    /// Site Roles
    /// </summary>
    [Flags]
    public enum SiteRoles
    {
        None                = 0,
        Read                = 2,
        Write               = 4,
        Publish             = 8,
    }
    /// <summary>
    /// Collection Type
    /// </summary>
    public enum CollectionType
    {
        Market              = 0,
        Culture             = 1,
        Site                = 2,
        PagesGroup          = 3,
        Page                = 4,
        Component           = 5,
    }
    /// <summary>
    /// Stylesheet Type
    /// </summary>
    public enum StylesheetType
    {
        InlineCss           = 0,
        InlineSass          = 1,
        InlineLess          = 2,
        Css                 = 10,
        Sass                = 11,
        Less                = 12,
    }
    /// <summary>
    /// Stylesheet Media
    /// </summary>
    public enum StylesheetMedia
    {
        All                 = 0,
        Screen              = 1,
        Print               = 2,
        Speech              = 3,
    }
    /// <summary>
    /// Script Type
    /// </summary>
    public enum ScriptType 
    {
        InlineJs            = 0,
        InlineTypeScript    = 1,
        Js                  = 10,
        TypeScript          = 11,
    }
    /// <summary>
    /// Script Load
    /// </summary>
    public enum ScriptLoad 
    {
        Normal              = 0,
        Async               = 1,
        Defer               = 2,
    }
    /// <summary>
    /// Publish Status
    /// </summary>
    public enum PublishStatus 
    {
        Unpublished         = 0,
        Published           = 1,
        Deleted             = 100,
    }
    /// <summary>
    /// Robots Tag
    /// </summary>
    public enum RobotsTag
    {
        IndexFollow         = 0,
        NoIndexFollow       = 1,
        IndexNoFollow       = 2,
        NoIndexNoFollow     = 3,
    }
    /// <summary>
    /// Component Placeholder Type
    /// </summary>
    [Flags]
    public enum ComponentPlaceholderType
    {
        Layout              = 0,
        Header              = 10,
        HeaderTags          = 11,
        Footer              = 20,
        Content             = 30,
    }
    /// <summary>
    /// Component Content Type
    /// </summary>
    public enum ComponentContentType 
    {
        Html                = 0,
        Url                 = 1,
    }
    /// <summary>
    /// Value Option
    /// </summary>
    public enum ValueOption
    {
        Include             = 0,
        Exclude             = 1,
    }
    /// <summary>
    /// Value Type
    /// </summary>
    public enum ValueType
    {
        Unknown             = 0,
        Text                = 1,
        Bool                = 2,
        Integer             = 3,
        Float               = 4,
        Decimal             = 5,
    }
    /// <summary>
    /// Redirect Code
    /// </summary>
    public enum RedirectCode 
    {
        Permanently         = 301,
        Temporarily         = 302,
    }
    /// <summary>
    /// Message Type
    /// </summary>
    public enum MessageType 
    {
        Info                = 0,
        Warning             = 1,
        Error               = 2,
    }
    /// <summary>
    /// Page Instance Selection Mode
    /// </summary>
    public enum PageInstanceSelectionMode
    {
        First               = 0,
        Random              = 1,
    }
}
