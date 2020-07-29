using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Haceau.JSON.test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------------ JSON1 ------------");
            JSON1();
            Console.WriteLine("------------ JSON2 ------------");
            JSON2();
            Console.WriteLine("------------ JSON3 ------------");
            JSON3();
            Console.WriteLine("------------ JSON4 ------------");
            JSON4();
        }

        static void JSON1()
        {
            string JSONString = "{\"Array\": [\"字符串\", false], \"number\": 123, \"double\": 999.888, \"dgfsrfe\": null}";
            Console.WriteLine($"Input: {JSONString}");
            ReadJSON readJSON = new ReadJSON();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            readJSON.Parser(JSONString);
            sw.Stop();
            Console.WriteLine($"一次所用时间：{sw.ElapsedMilliseconds} ms");
            Dictionary<string, object> obj = readJSON.Parser(JSONString);
            foreach (var item in obj)
            {
                Console.WriteLine($"{item.Key}:{item.Value}");
            }
            Console.WriteLine("{");
            Console.WriteLine("\t\"Array\": [");
            Console.WriteLine($"\t\t\"{((List<object>)obj["Array"])[0]}\",");
            Console.WriteLine($"\t\t{((List<object>)obj["Array"])[1]}");
            Console.WriteLine("\t],");
            Console.WriteLine($"\t\"number\":{obj["number"]},");
            Console.WriteLine($"\t\"double\":{obj["double"]},");
            Console.WriteLine($"\t\"dgfsrfe\":{obj["dgfsrfe"] ?? "null"}");
            Console.WriteLine("}");
            Console.WriteLine($"转换为字符串:{WriteJSON.Write(obj)}");

            sw.Start();
            for (int i = 0; i < 100; ++i)
                readJSON.Parser(JSONString);
            sw.Stop();
            Console.WriteLine($"一百次所用时间：{sw.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }

        static void JSON2()
        {
            string JSONString = "{\"     Array\":[  \"字符串\", false],      \"number\":   " +
                "\n\n\n\t\r123, \"dgfsrfe\": null}";

            Console.WriteLine($"Input: {JSONString}");
            ReadJSON readJSON = new ReadJSON();
            Dictionary<string, object> obj = readJSON.Parser(JSONString);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            readJSON.Parser(JSONString);
            sw.Stop();
            Console.WriteLine($"所用时间：{sw.ElapsedMilliseconds} ms");
            Console.WriteLine("{");
            Console.WriteLine("\t\"     Array\": [");
            Console.WriteLine($"\t\t\"{((List<object>)obj["     Array"])[0]}\",");
            Console.WriteLine($"\t\t{((List<object>)obj["     Array"])[1]},");
            Console.WriteLine("\t],");
            Console.WriteLine($"\t\"number\":{obj["number"]},");
            Console.WriteLine($"\t\"dgfsrfe\":{obj["dgfsrfe"] ?? "null"}");
            Console.WriteLine("}");
            Console.WriteLine($"转换为字符串:{WriteJSON.Write(obj)}");
            sw.Start();
            for (int i = 0; i < 100; ++i)
                readJSON.Parser(JSONString);
            sw.Stop();
            Console.WriteLine($"一百次所用时间：{sw.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }

        static void JSON3()
        {
            string JSONString = "[true]";

            Console.WriteLine($"Input: {JSONString}");

            ReadJSON readJSON = new ReadJSON();
            List<object> obj = readJSON.Parser(JSONString);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            readJSON.Parser(JSONString);
            sw.Stop();

            Console.WriteLine($"所用时间：{sw.ElapsedMilliseconds} ms");
            Console.WriteLine("[");
            Console.WriteLine($"\t{obj[0]}");
            Console.WriteLine("]");
            Console.WriteLine($"转换为字符串:{WriteJSON.Write(obj)}");
            sw.Start();
            for (int i = 0; i < 100; ++i)
                readJSON.Parser(JSONString);
            sw.Stop();
            Console.WriteLine($"一百次所用时间：{sw.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }

        static void JSON4()
        {
            string JSONString = "{\"st\tring\": \"\t\t\\t\u1234\"}";

            Console.WriteLine($"Input: {JSONString}");

            ReadJSON readJSON = new ReadJSON();
            Dictionary<string, object> obj = readJSON.Parser(JSONString);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            readJSON.Parser(JSONString);
            sw.Stop();

            Console.WriteLine($"所用时间：{sw.ElapsedMilliseconds} ms");
            Console.WriteLine("{");
            Console.WriteLine($"\t\"st\tring\":\"{obj["st\tring"]}\"");
            Console.WriteLine("}");
            Console.WriteLine($"转换为字符串:{WriteJSON.Write(obj)}");
            sw.Start();
            for (int i = 0; i < 100; ++i)
                readJSON.Parser(JSONString);
            sw.Stop();
            Console.WriteLine($"一百次所用时间：{sw.ElapsedMilliseconds} ms");
            Console.ReadKey();
        }
    }
}