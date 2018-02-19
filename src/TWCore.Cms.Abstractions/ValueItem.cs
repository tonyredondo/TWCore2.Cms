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
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace TWCore.Cms
{
    /// <summary>
    /// Value Item
    /// </summary>
    [DataContract]
    public class ValueItem
    {
        /// <summary>
        /// String Value
        /// </summary>
        [DataMember]
        public string SValue { get; set; }
        /// <summary>
        /// Bool Value
        /// </summary>
        [DataMember]
        public bool? BValue { get; set; }
        /// <summary>
        /// Integer Value
        /// </summary>
        [DataMember]
        public int? IValue { get; set; }
        /// <summary>
        /// Float Value
        /// </summary>
        [DataMember]
        public float? FValue { get; set; }
        /// <summary>
        /// Decimal Value
        /// </summary>
        [DataMember]
        public decimal? DValue { get; set; }
        /// <summary>
        /// Value Option
        /// </summary>
        [DataMember]
        public ValueOption Option { get; set; }
        /// <summary>
        /// Value Type
        /// </summary>
        [DataMember]
        public ValueType Type { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public object Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch (Type)
                {
                    case ValueType.Text:
                        return SValue;
                    case ValueType.Bool:
                        return BValue;
                    case ValueType.Integer:
                        return IValue;
                    case ValueType.Float:
                        return FValue;
                    case ValueType.Decimal:
                        return DValue;
                    case ValueType.Unknown:
                        return null;
                    default:
                        return null;
                }
            }
        }

        #region .ctor
        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem() { }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(object value, ValueOption option = ValueOption.Include)
        {
            Option = option;
            switch(value)
            {
                case string svalue:
                    SValue = svalue;
                    Type = ValueType.Text;
                    break;
                case bool bvalue:
                    BValue = bvalue;
                    Type = ValueType.Bool;
                    break;
                case int ivalue:
                    IValue = ivalue;
                    Type = ValueType.Integer;
                    break;
                case float fvalue:
                    FValue = fvalue;
                    Type = ValueType.Float;
                    break;
                case decimal dvalue:
                    DValue = dvalue;
                    Type = ValueType.Decimal;
                    break;
                default:
                    Type = ValueType.Unknown;
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(string value, ValueOption option = ValueOption.Include)
        {
            SValue = value;
            Option = option;
            Type = ValueType.Text;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(bool value, ValueOption option = ValueOption.Include)
        {
            BValue = value;
            Option = option;
            Type = ValueType.Bool;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(int value, ValueOption option = ValueOption.Include)
        {
            IValue = value;
            Option = option;
            Type = ValueType.Integer;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(float value, ValueOption option = ValueOption.Include)
        {
            FValue = value;
            Option = option;
            Type = ValueType.Float;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(decimal value, ValueOption option = ValueOption.Include)
        {
            DValue = value;
            Option = option;
            Type = ValueType.Decimal;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueItem(ValueOption option)
        {
            Option = option;
        }
        #endregion

        #region Operators
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueItem(string value)
            => new ValueItem(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueItem(bool value)
            => new ValueItem(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueItem(int value)
            => new ValueItem(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueItem(float value)
            => new ValueItem(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueItem(decimal value)
            => new ValueItem(value);
        #endregion
    }
}
