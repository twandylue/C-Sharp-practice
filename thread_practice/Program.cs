using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace thread_practice
{
    class Program
    {
        // normal
        // static void Main()
        // {
        //     var producer = Task.Run(() => Producer()); //改成TPL的方式
        //     var consumer = Task.Run(() => Consumer());
        //     consumer.Wait();
        // }
        
        // await 重點 Main() 前方的Task
        // static async Task Main()
        // {
        //     await task_pratice.awaitRun();
        // }


        // task_list
        static void Main()
        {
            task_pratice.Run();
        }
        private static BlockingCollection<int> data = new BlockingCollection<int>(); //共享資料

        private static void Producer() //生產者

        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(100);
                data.Add(i);
            }
        }

        private static void Consumer() //消費者
        {
            foreach (var item in data.GetConsumingEnumerable())
            {
                Console.WriteLine(item);
                Thread.Sleep(100);
            }
        }
    }
}