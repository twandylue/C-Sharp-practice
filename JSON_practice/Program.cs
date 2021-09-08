using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JSON_practice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            var forSerialize = new Person
            {
                age = 27,
                name = "Andy"
            };
            var options = new JsonSerializerOptions { WriteIndented = true }; // 格式化
            string jsonString = JsonSerializer.Serialize<Person>(forSerialize, options); // 序列化
            Console.WriteLine(jsonString);

            string jsonStringRaw = @"{ ""age"": 27, ""name"": ""andy""}";
            Person forDeseiralize = JsonSerializer.Deserialize<Person>(jsonStringRaw); // 反序列化
            Console.WriteLine(forDeseiralize.age);
            Console.WriteLine(forDeseiralize.name);
        }
    }

    class Person
    {
        public int age { get; set; }
        public string name { get; set; }
    }
}
