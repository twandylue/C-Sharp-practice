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
            var test = new Person {
                age = 27,
                name = "Andy"
            };
            var options = new JsonSerializerOptions{WriteIndented = true}; // 格式化
            string jsonString = JsonSerializer.Serialize<Person>(test, options);
            Console.WriteLine(jsonString);
        }
    }
    
    class Person {
        public int age { get; set;}
        public string name {get; set;}
    }
}
