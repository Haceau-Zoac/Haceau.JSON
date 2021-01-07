using Haceau.Json.New;
using NuGet.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Haceau.Json.New.Test
{
    [TestFixture]
    public class JsonConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSerializeNull()
        {
            Assert.AreEqual(null, JsonConverter.SerializeNull("null"));
            Assert.AreEqual(null, JsonConverter.SerializeNull("null  "));
            Assert.AreEqual(null, JsonConverter.SerializeNull("nullqwqwqwqdafs gargwdsgda "));

            Assert.Catch<ArgumentNullException>(() =>
            {
                JsonConverter.SerializeNull("               ");
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                JsonConverter.SerializeNull(string.Empty);
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                JsonConverter.SerializeNull(null);
            });

            Assert.Catch<FormatException>(() =>
            {
                JsonConverter.SerializeNull("n");
            });

            Assert.Catch<FormatException>(() =>
            {
                JsonConverter.SerializeNull(" null");
            });
        }

        [Test]
        public void TestSerializeBool()
        {
            Assert.AreEqual(true, JsonConverter.SerializeBool("true"));
            Assert.AreEqual(false, JsonConverter.SerializeBool("false"));
            Assert.AreEqual(true, JsonConverter.SerializeBool("true "));

            Assert.Catch<ArgumentNullException>(() =>
            {
                JsonConverter.SerializeBool("               ");
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                JsonConverter.SerializeBool(string.Empty);
            });

            Assert.Catch<ArgumentNullException>(() =>
            {
                JsonConverter.SerializeBool(null);
            });

            Assert.Catch<FormatException>(() =>
            {
                JsonConverter.SerializeBool("t");
            });

            Assert.Catch(typeof(FormatException), () =>
            {
                JsonConverter.SerializeBool(" false");
            });
        }

        [Test]
        public void TestSerializeString()
        {
            Assert.AreEqual("giaogiao", JsonConverter.SerializeString("\"giaogiao\""));
            Assert.AreEqual("\tqwq\u1234\"\"", JsonConverter.SerializeString("\"\\tqwq\\u1234\\\"\\\"\""));
        }

        [Test]
        public void TestSerializeChar()
        {
            Assert.AreEqual('\\', JsonConverter.SerializeChar("'\\\\'"));
        }

        [Test]
        public void TestSerializeNumber()
        {
            Assert.AreEqual(12, JsonConverter.SerializeNumber("12"));
            Assert.AreEqual(1232.5, JsonConverter.SerializeNumber("1232.50000"));
            Assert.AreEqual(-10086, JsonConverter.SerializeNumber("-10086.0"));

        }

        [Test]
        public void TestSerializePair()
        {
            Person person = new();
            JsonConverter.SerializePair(ref person, @"""name"": ""haceau""");
            Assert.AreEqual("haceau", person.name);

            JsonConverter.SerializePair(ref person, @"""age"": 12");
            Assert.AreEqual(12, person.age);

            JsonConverter.SerializePair(ref person, @"""Name"": ""haceau""");
            Assert.AreEqual("haceau", person.Name);

            JsonConverter.SerializePair(ref person, @"""Age"": 23");
            Assert.AreEqual(23, person.Age);
        }

        [Test]
        public void TestSerializeObject()
        {
            {
                Dictionary<string, object> pairs = JsonConverter.SerializeObject(@"{""age"":12,""name"":""haceau"",""Age"":11,""name"":""zoac"",""names"": 
                                                                                    [""kite"",""hello world"", {""test"":""pass""}]}");
                Assert.AreEqual(pairs["age"], 12);
                Assert.AreNotEqual(pairs["name"], "haceau");
                Assert.AreEqual(pairs["Age"], 11);
                Assert.AreEqual(pairs["name"], "zoac");
                Dictionary<string, object> dic = new();
                dic.Add("test", "pass");
                Assert.AreEqual(pairs["names"], new object[] { "kite", "hello world", dic });
            }
        }
    }

    class Person
    {
        public int age;
        public string name;

        public int Age { get; set; }
        public string Name { get; set; }
    }
}