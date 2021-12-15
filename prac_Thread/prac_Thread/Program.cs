using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BasicThread
{
    class Program
    {
        static void DoSomething()
        {
            for (int i = 0; i < 5; i ++)
            {
                Console.WriteLine($"DoSomething : {i}");
                Thread.Sleep(10); // CPU 점유 양보
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(DoSomething));

            Console.WriteLine("Starting Thread");
            t1.Start();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Main : {i}");
                Thread.Sleep(10); 
            }

            Console.WriteLine("Waiting until thread stops");
            t1.Join(); // t1이 끝날 때 까지 대기

            Console.WriteLine("Finished");

            /* Result
                Starting Thread
                Main : 0
                DoSomething : 0
                DoSomething : 1
                Main : 1
                Main : 2
                DoSomething : 2
                DoSomething : 3
                Main : 3
                Main : 4
                DoSomething : 4
                Waiting until thread stops
                Finished
            */

        }
    }
}
