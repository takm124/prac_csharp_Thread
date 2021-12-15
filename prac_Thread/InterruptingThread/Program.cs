using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InterruptingThread
{
    class SideTask
    {
        int count;

        public SideTask(int count)
        {
            this.count = count;
        }

        public void KeepAlive()
        {
            try
            {
                Console.WriteLine("Running thread isn't gonna be interrupted");
                Thread.SpinWait(1000000000); // 반복문 돌 때 대기상태

                while (count > 0)
                {
                    Console.WriteLine($"{count--} left");

                    Console.WriteLine("Entering into WaitJoinSleep State....");
                    Thread.Sleep(10);
                }
                Console.WriteLine("Count : 0");
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Clearing resource");
            }
        }
    }

    class MainApp
    {
        static void Main(string[] args)
        {
            SideTask task = new SideTask(100);
            Thread t1 = new Thread(new ThreadStart(task.KeepAlive));
            t1.IsBackground = false;

            Console.WriteLine("Starting thread");
            t1.Start();

            Thread.Sleep(100);

            Console.WriteLine("Interrupting thread..");
            t1.Interrupt();

            Console.WriteLine("Wating until thread stops");
            t1.Join();

            Console.WriteLine("Finished");
            
        }
    }
    /* Result
        Starting thread
        Running thread isn't gonna be interrupted
        Interrupting thread..
        Wating until thread stops
        100 left
        Entering into WaitJoinSleep State....
        System.Threading.ThreadInterruptedException: 스레드가 대기 상태에서 인터럽트되었습니다.
           위치: System.Threading.Thread.SleepInternal(Int32 millisecondsTimeout)
           위치: System.Threading.Thread.Sleep(Int32 millisecondsTimeout)
           위치: InterruptingThread.SideTask.KeepAlive() 파일 C:\Users\user\Documents\Visual Studio 2015\Projects\prac_Thread\InterruptingThread\Program.cs:줄 31
        Clearing resource
        Finished

    */
}
