using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace thread_practice
{
    public class task_pratice
    {
        public static async Task awaitRun()
        {
            await Task.Run(() =>
            {
                for (int i = 1; i < 1000; i++)
                {
                    Console.WriteLine($"Number: {i}");
                }
            });
        }
        public static void Run()
        {
            List<Task> task_list = new List<Task>();
            for (int i = 0; i < 10; i++) {
                Task t = Task.Run(()=>{
                    Thread.Sleep(i*1000);
                    Console.WriteLine("test");
                });
                task_list.Add(t); // thread pool 的概念
            }
            foreach (var task in task_list) task.Wait();
        }
    }
}