using System;
using System.Collections.Generic;
using System.Linq;

namespace Haceau.JSON
{
    public static class WriteJSON
    {
        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>字符串</returns>
        public static string Write(object obj) =>
            obj.GetType() == typeof(List<object>) ? NextArrayToken((List<object>)obj) : NextObjectToken((Dictionary<string, object>)obj);

        /// <summary>
        /// 获取数组的下一个token
        /// </summary>
        /// <returns>token</returns>
        private static string NextArrayToken(List<object> array)
        {
            string result = "[";
            foreach (var item in array)
            {
                if ((item == null ? typeof(Nullable) : item.GetType()) == typeof(Dictionary<string, object>))
                    result += NextObjectToken((Dictionary<string, object>)item);
                else if ((item == null ? typeof(Nullable) : item.GetType()) == typeof(string))
                    result += '"' + (string)item + '"';
                else
                    result += (item ?? "null");
                result += ",";
            }
            result = result.Remove(result.ToArray().Length - 1);
            result += "]";
            return result;
        }

        /// <summary>
        /// 获取对象的下一个token
        /// </summary>
        /// <returns>token</returns>
        private static string NextObjectToken(Dictionary<string, object> obj)
        {
            string result = "{";
            foreach (var item in obj)
            {
                if ((item.Value == null ? typeof(Nullable) : item.Value.GetType()) == typeof(List<object>))
                    result += '"' + item.Key + '"' + ":" + NextArrayToken((List<object>)item.Value);
                else if ((item.Value == null ? typeof(Nullable) : item.Value.GetType()) == typeof(string))
                    result += '"' + item.Key + '"' + ":" + '"' + item.Value + '"';
                else
                    result += '"' + item.Key + '"' + ":" + (item.Value ?? "null");
                result += ",";
            }
            result = result.Remove(result.ToArray().Length - 1);
            result += "}";
            return result;
        }
    }
}
