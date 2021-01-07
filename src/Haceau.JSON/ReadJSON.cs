using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Haceau.JSON
{
    public class ReadJSON
    {
        /// <summary>
        /// 下标
        /// </summary>
        private int index = 0;
        /// <summary>
        /// JSON
        /// </summary>
        public string JSON;

        /// <summary>
        /// 初始化，以使用一个对象读取多次JSON
        /// </summary>
        public void Init(string JSON)
        {
            index = 0;
            InitJSON(JSON);
        }

        /// <summary>
        /// 格式化JSON
        /// </summary>
        /// <returns>格式化后的JSON</returns>
        private void InitJSON(string JSON)
        {
            // 去除空格
            for (int i = 0; i < JSON.Length; ++i)
            {
                // 如果在字符串内就不去除
                if (JSON[i] == '"')
                {
                    this.JSON += '"' + JSON.Substring(++i, JSON.IndexOf('\"', i) - i);
                    i = JSON.IndexOf('\"', i);
                }
                this.JSON += new Regex("[ \t\r\n]").Replace(JSON[i].ToString(), "");
            }
        }

        /// <summary>
        /// 获取下一个token
        /// </summary>
        /// <returns>获取到的token</returns>
        private object NextToken()
        {
            switch (Char())
            {
                case '[':
                    NextChar();
                    return ReadArray();
                case '{':
                    NextChar();
                    return ReadObject();
                case '"':
                    NextChar();
                    return ReadString();
                case 'n':
                    return ReadNull();
                case ',':
                    NextChar();
                    return NextToken();
                default:
                    {
                        // 是数字
                        if (Regex.IsMatch(Char().ToString(), "[0-9]"))
                        {
                            return ReadNumber();
                        }
                        // 是布尔值
                        if (Regex.IsMatch(Char().ToString(), "[tf]"))
                        {
                            return ReadBoolean();
                        }
                        throw new FormatException("错误的JSON格式。");
                    }
            }
        }

        /// <summary>
        /// 读取数组
        /// </summary>
        /// <returns>读取的token</returns>
        private List<object> ReadArray()
        {
            // 初始化token
            List<object> array = new List<object>();

            // 如果数组没有结束就一直读取token
            while (Char() != ']')
                array.Add(NextToken());
            NextChar();
            return array;
        }

        /// <summary>
        /// 读取对象
        /// </summary>
        /// <returns>读取到的token</returns>
        private Dictionary<string, object> ReadObject()
        {
            // 初始化token
            Dictionary<string, object> obj = new Dictionary<string, object>();

            // 如果对象没有结束就一直读取token
            while (true)
            {
                NextChar();
                string key = ReadString();
                // 如果不是 key:value 格式则抛出异常
                if (Char() != ':')
                    throw new FormatException("错误的JSON格式。");
                NextChar();
                object value = NextToken();
                obj.Add(key, value);
                // 如果结尾没有','则跳出循环
                if (Char() != ',')
                    break;
                NextChar();
            }
            NextChar();
            return obj;
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        /// <returns>读取到的token</returns>
        private string ReadString()
        {
            // 获取字符串
            string str = JSON.Substring(index, JSON.IndexOf('"', index) - index);
            index = JSON.IndexOf('"', index) + 1;
            return str;
        }

        /// <summary>
        /// 读取数字
        /// </summary>
        /// <returns>读取到的token</returns>
        private double ReadNumber()
        {
            double num;

            // 读取数字
            bool isDouble = false;
            string number = "";
            while (Regex.IsMatch(Char().ToString(), "[0-9]") || Char() == '.')
            {
                if (Char() == '.' && isDouble)
                    throw new FormatException("错误的JSON格式。");
                else if (Char() == '.')
                    isDouble = true;
                number += Char();
                NextChar();
            }
            num = double.Parse(number);
            return num;
        }

        /// <summary>
        /// 读取布尔值
        /// </summary>
        /// <returns>读取到的token</returns>
        private bool ReadBoolean()
        {
            bool boolean;

            if (JSON.Substring(index, 4) == "true")
            {
                boolean = true;
                index += 4;
            }
            else if (JSON.Substring(index, 5) == "false")
            {
                boolean = false;
                index += 5;
            }
            else
                throw new FormatException("错误的JSON格式。");

            return boolean;
        }

        /// <summary>
        /// 读取空值
        /// </summary>
        /// <returns>读取到的token</returns>
        private object ReadNull()
        {
            object nullValue;

            if (JSON.Substring(index, 4) == "null")
                nullValue = null;
            else
                throw new FormatException("错误的JSON格式。");

            for (int i = 0; i < 4; ++i)
                NextChar();

            return nullValue;
        }

        /// <summary>
        /// 下一个字符
        /// </summary>
        private void NextChar() => ++index;

        /// <summary>
        /// 当前字符
        /// </summary>
        private char Char() => JSON[index];

        /// <summary>
        /// 语法分析
        /// </summary>
        /// <returns>分析结果</returns>
        public dynamic Parser(string JSON)
        {
            Init(JSON);
            if (Char() == '{')
                return ParserObject();
            else
                return ParserArray();
        }

        /// <summary>
        /// 数组的语法分析
        /// </summary>
        /// <returns>分析结果</returns>
        private List<object> ParserArray() => 
            (List<object>)NextToken();

        /// <summary>
        /// 对象的语法分析
        /// </summary>
        /// <returns>分析结果</returns>
        private Dictionary<string, object> ParserObject() =>
            (Dictionary<string, object>)NextToken();
    }
}
