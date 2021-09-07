using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Asynchronous_programming
{
    public class Producer
    {
        public static async Task produce1()
        {
            // first use await
            await Task.Run(() =>
            {
                Console.WriteLine("Completed!");
                Thread.Sleep(2000);
            });
        }

        public static void produce2()
        {
            // second use Wait
            Task t = Task.Run(() =>
            {
                Console.WriteLine("Completed!");
                Thread.Sleep(5000);
            });
            t.Wait();
        }

        // third
        public static void produce3()
        {
            Thread t = new Thread(() =>
            {
                Console.WriteLine("Completed!");
                Thread.Sleep(3000);
            });
            t.Start();
            t.Join();
        }
    }
}