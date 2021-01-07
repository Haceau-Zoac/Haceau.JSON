// Copyright © Haceau and Contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// 新的 Json 序列化 & 反序列化设施。
/// </summary>
namespace Haceau.Json.New
{
    /// <summary>
    /// Json 转换器，包含序列化和反序列化 Json 的方法。
    /// </summary>
    public static class JsonConverter
    {
        /// <summary>
        /// 反序列化对象。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json object 格式。
        /// </exception>
        public static Dictionary<string, object> SerializeObject(string json)
        {
            int ix = 0;
            return SerializeObject(json, ref ix);
        }
        /// <summary>
        /// 反序列化对象。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json object 格式。
        /// </exception>
        public static Dictionary<string, object> SerializeObject(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '{') throw new FormatException("json 格式错误。");

            Dictionary<string, object> result = new();
            bool next = true;
            for (++ix; ix < json.Length; ++ix)
            {
                switch (json[ix])
                {
                    case '}':
                        return result;
                    case '"':
                        if (next)
                        {
                            KeyValuePair<string, object> pair = SerializePair(json, ref ix);
                            result[pair.Key] = pair.Value;
                        }
                        else throw new FormatException("json 格式错误。");
                        next = false;
                        break;
                    case ',':
                        if (next) throw new FormatException("json 格式错误。");
                        next = true;
                        break;
                    default:
                        if (!char.IsWhiteSpace(json[ix])) throw new FormatException("json 格式错误。");
                        break;
                }
            }
            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化对象。
        /// </summary>
        /// <param name="obj">反序列化进的对象。</param>
        /// <param name="json">要反序列化的 json。</param>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json object 格式。
        /// </exception>
        public static void SerializeObject<T>(ref T obj, string json)
        {
            int ix = 0;
            SerializeObject(ref obj, json, ref ix);
        }
        /// <summary>
        /// 反序列化对象。
        /// </summary>
        /// <param name="obj">反序列化进的对象。</param>
        /// <param name="json">要反序列化的 json。</param>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json object 格式。
        /// </exception>
        private static void SerializeObject<T>(ref T obj, string json, ref int ix)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '{') throw new FormatException("json 格式错误。");

            bool next = true;
            for (++ix; ix < json.Length; ++ix)
            {
                switch (json[ix])
                {
                    case '}':
                        return;
                    case '"':
                        if (next) { SerializePair(ref obj, json, ref ix); next = false; }
                        else throw new FormatException("json 格式错误。");
                        break;
                    case ',':
                        if (!next) throw new FormatException("json 格式错误。");
                        next = true;
                        break;
                    default:
                        if (!char.IsWhiteSpace(json[ix])) throw new FormatException("json 格式错误。");
                        break;
                }
            }
            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化数组。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json array 格式。
        /// </exception>
        public static List<object> SerializeArray(string json)
        {
            int ix = 0;
            return SerializeArray(json, ref ix);
        }
        /// <summary>
        /// 反序列化数组。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json array 格式。
        /// </exception>
        private static List<object> SerializeArray(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '[') throw new FormatException("json 格式错误。");

            List<object> result = new();
            bool next = true;
            for (++ix; ix < json.Length; ++ix)
            {
                switch (json[ix])
                {
                    case '"':
                        if (next) { result.Add(SerializeString(json, ref ix)); next = false; }
                        else throw new FormatException("json 格式错误。");
                        break;
                    case '\'':
                        if (next) { result.Add(SerializeString(json, ref ix)); next = false; }
                        else throw new FormatException("json 格式错误。");
                        break;
                    case '{':
                        if (next) { result.Add(SerializeObject(json, ref ix)); next = false; }
                        else throw new FormatException("json 格式错误。");
                        break;
                    case '[':
                        if (next) { result.Add(SerializeArray(json, ref ix)); next = false; }
                        else throw new FormatException("json 格式错误。");
                        break;
                    case ']':
                        return result;
                    case ',':
                        if (next) throw new FormatException("json 格式错误。");
                        next = true;
                        break;
                    default:
                        if (!char.IsWhiteSpace(json[ix])) throw new FormatException("json 格式错误。");
                        if (char.IsDigit(json[ix]))
                        {
                            if (next) { result.Add(SerializeNumber(json, ref ix)); next = false; }
                            else throw new FormatException("json 格式错误。");
                        }
                        break;
                }
            }
            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化键值对。
        /// </summary>
        /// <param name="obj">反序列化进的对象。</param>
        /// <param name="json">要反序列化的 json。</param>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json pair 格式。
        /// </exception>
        public static void SerializePair<T>(ref T obj, string json)
        {
            int ix = 0;
            SerializePair(ref obj, json, ref ix);
        }
        /// <summary>
        /// 反序列化键值对。
        /// </summary>
        /// <param name="obj">反序列化进的对象。</param>
        /// <param name="json">要反序列化的 json。</param>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json pair 格式。
        /// </exception>
        private static void SerializePair<T>(ref T obj, string json, ref int ix)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '"') throw new FormatException("json 格式错误。");

            string key = SerializeString(json, ref ix);
            bool isObject = false;
            for (++ix; ix < json.Length; ++ix)
            {
                switch (json[ix])
                {
                    case ':':
                        if (isObject) throw new FormatException("json 格式错误。");
                        isObject = true;
                        break;
                    case '"':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        SetProperty(ref obj, key, SerializeString(json, ref ix));
                        return;
                    case '\'':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        SetProperty(ref obj, key, SerializeChar(json, ref ix));
                        return;
                    case 't':
                    case 'f':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        SetProperty(ref obj, key, SerializeBool(json, ref ix));
                        return;
                    case 'n':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        SetProperty(ref obj, key, SerializeNull(json, ref ix));
                        return;
                    case '[':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        SetProperty(ref obj, key, SerializeArray(json, ref ix));
                        return;
                    default:
                        if (!char.IsWhiteSpace(json[ix]))
                        {
                            if (!isObject) throw new FormatException("json 格式错误。");
                            if (char.IsDigit(json[ix]))
                            {
                                SetProperty(ref obj, key, SerializeNumber(json, ref ix));
                                return;
                            }
                        }
                        break;
                }

            }
            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化键值对。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json pair 格式。
        /// </exception>
        public static KeyValuePair<string, object> SerializePair(string json)
        {
            int ix = 0;
            return SerializePair(json, ref ix);
        }
        /// <summary>
        /// 反序列化键值对。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串 |或| obj 为 null。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json pair 格式。
        /// </exception>
        private static KeyValuePair<string, object> SerializePair(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '"') throw new FormatException("json 格式错误。");

            string key = SerializeString(json, ref ix);
            bool isObject = false;
            for (++ix; ix < json.Length; ++ix)
            {
                switch (json[ix])
                {
                    case ':':
                        if (isObject) throw new FormatException("json 格式错误。");
                        isObject = true;
                        break;
                    case '"':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        return new(key, SerializeString(json, ref ix));
                    case '\'':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        return new(key, SerializeChar(json, ref ix));
                    case 't':
                    case 'f':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        return new(key, SerializeBool(json, ref ix));
                    case 'n':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        return new(key, SerializeNull(json, ref ix));
                    case '[':
                        if (!isObject) throw new FormatException("json 格式错误。");
                        return new(key, SerializeArray(json, ref ix));
                    default:
                        if (!char.IsWhiteSpace(json[ix]))
                        {
                            if (!isObject) throw new FormatException("json 格式错误。");
                            if (char.IsDigit(json[ix])) return new(key, SerializeNumber(json, ref ix));
                        }
                        break;
                }

            }
            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化字符串。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json string 格式。
        /// </exception>
        public static string SerializeString(string json)
        {
            int ix = 0;
            return SerializeString(json, ref ix);
        }
        /// <summary>
        /// 反序列化字符串。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json string 格式。
        /// </exception>
        private static string SerializeString(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '"') throw new FormatException("json 格式错误。");

            string str = string.Empty;
            for (++ix; ix < json.Length; ++ix)
            {
                if (json[ix] == '"' && json[ix - 1] != '\\') return Regex.Unescape(str);
                str += json[ix];
            }
            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化数字。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json string 格式。
        /// </exception>
        public static double SerializeNumber(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (!char.IsDigit(json[0]) && json[0] != '-') throw new FormatException("json 格式错误。");

            string number = json[0].ToString();
            for (int ix = 1; ix < json.Length; ++ix)
            {
                if (json[ix] != '.' &&
                    !char.IsDigit(json[ix])) break;
                number += json[ix];
            }
            if (!double.TryParse(number, out double ret)) throw new FormatException("json 格式错误。");
            return ret;
        }        
        /// <summary>
        /// 反序列化数字。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json string 格式。
        /// </exception>
        private static double SerializeNumber(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (!char.IsDigit(json[ix]) && json[ix] != '-') throw new FormatException("json 格式错误。");

            string number = json[ix].ToString();
            for (++ix;
                ix < json.Length &&
                json[ix] != '.' && char.IsDigit(json[ix]);
                ++ix)
            {
                number += json[ix];
            }
            if (!double.TryParse(number, out double ret)) throw new FormatException("json 格式错误。");
            --ix;
            return ret;
        }

        /// <summary>
        /// 反序列化字符。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json char 格式。
        /// </exception>
        public static char SerializeChar(string json)
        {
            int ix = 0;
            return SerializeChar(json, ref ix);
        }        
        /// <summary>
        /// 反序列化字符。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json char 格式。
        /// </exception>
        private static char SerializeChar(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] != '\'') throw new FormatException("json 格式错误。");

            string ch = string.Empty;
            for (++ix; ix < json.Length; ++ix)
            {
                if (json[ix] == '\'' && json[ix - 1] != '\'')
                {
                    ch = Regex.Unescape(ch);
                    if (ch.Length > 1) throw new FormatException("json 格式错误。");
                    return ch[ix];
                }

                ch += json[ix];
            }

            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化布尔。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json bool 格式。
        /// </exception>
        public static bool SerializeBool(string json)
        {
            int ix = 0;
            return SerializeBool(json, ref ix);
        }     
        /// <summary>
        /// 反序列化布尔。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json bool 格式。
        /// </exception>
        private static bool SerializeBool(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json[ix] == 't' && json.Length >= ix + 4 && json[..(ix + 4)] == "true") return true;
            if (json[ix] == 'f' && json.Length >= ix + 5 && json[..(ix + 5)] == "false") return false;

            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 反序列化空。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json null 格式。
        /// </exception>
        public static object SerializeNull(string json)
        {
            int ix = 0;
            return SerializeNull(json, ref ix);
        }
        /// <summary>
        /// 反序列化空。
        /// </summary>
        /// <param name="json">要反序列化的 json。</param>
        /// <returns>反序列化结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// json 为空字符串 |或| json 为空白字符串。
        /// </exception>
        /// <exception cref="FormatException">
        /// json 的格式不符合 json null 格式。
        /// </exception>
        private static object SerializeNull(string json, ref int ix)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new ArgumentNullException(nameof(json));
            if (json.Length >= ix + 4 && json[..(ix + 4)] == "null") return null;

            throw new FormatException("json 格式错误。");
        }

        /// <summary>
        /// 设置属性。
        /// </summary>
        /// <param name="obj">要设置属性的对象。</param>
        /// <param name="property">要设置的属性。</param>
        /// <param name="value">要设置的值</param>
        private static void SetProperty<T>(ref T obj, string property, object value)
        {
            MemberInfo[] memberInfos = typeof(T).GetMember(property);
            foreach (var memberInfo in memberInfos)
            {
                if (memberInfo.MemberType == MemberTypes.Field)
                {
                    FieldInfo fieldInfo = (FieldInfo)memberInfo;
                    Type fieldType = fieldInfo.FieldType;
                    fieldInfo.SetValue(obj, Convert.ChangeType(value, fieldType));
                    return;
                }
                if (memberInfo.MemberType == MemberTypes.Property)
                {
                    PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                    Type fieldType = propertyInfo.PropertyType;
                    propertyInfo.SetValue(obj, Convert.ChangeType(value, fieldType));
                    return;
                }
            }
        }
    }
}
