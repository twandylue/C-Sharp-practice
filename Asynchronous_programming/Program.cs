using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Asynchronous_programming
{
    class Program
    {
        // first
        // static async Task Main(string[] args)
        // {
        //     await Producer.produce1();
        //     Console.WriteLine("Hello World!");
        // }

        // second and third
        static void Main(string[] args)
        {
            // Producer.produce2();
            Producer.produce3();
            Console.WriteLine("Hello World!");
        }
    }
}
