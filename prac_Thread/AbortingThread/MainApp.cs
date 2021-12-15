using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbortingThread
{
    /* 
        Abort는 사용하지 않는 것이 좋다.
        abort를 호출하더라도 동작하던 스레드가 즉시 종료된다는 보장이 없기 때문이다.
        CLR이 사용하던 thread에 exception을 던지면서 멈추게됨
        또한 한 thread가 자원 선점상태에서 abort 하게되면 다른 thread에 영향을 미친다.
    */

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
                while (count > 0)
                {
                    Console.WriteLine($"{count--} left");
                    Thread.Sleep(10);
                }
                Console.WriteLine("Count : 0");
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine(e);
                Thread.ResetAbort();
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

            Console.WriteLine("Aborting Thread");
            t1.Abort();

            Console.WriteLine("Wating until thread stops");
            t1.Join();

            Console.WriteLine("Finished");
        }
    }
    /* Result
        Starting thread
        100 left
        99 left
        98 left
        97 left
        96 left
        95 left
        94 left
        Aborting Thread
        93 left
        System.Threading.ThreadAbortException: 스레드가 중단되었습니다.
           위치: Microsoft.Win32.Win32Native.WriteFile(SafeFileHandle handle, Byte* bytes, Int32 numBytesToWrite, Int32& numBytesWritten, IntPtr mustBeZero)
           위치: System.IO.__ConsoleStream.WriteFileNative(SafeFileHandle hFile, Byte[] bytes, Int32 offset, Int32 count, Boolean useFileAPIs)
           위치: System.IO.__ConsoleStream.Write(Byte[] buffer, Int32 offset, Int32 count)
           위치: System.IO.StreamWriter.Flush(Boolean flushStream, Boolean flushEncoder)
           위치: System.IO.StreamWriter.Write(Char[] buffer, Int32 index, Int32 count)
           위치: System.IO.TextWriter.WriteLine(String value)
           위치: System.IO.TextWriter.SyncTextWriter.WriteLine(String value)
           위치: System.Console.WriteLine(String value)
           위치: AbortingThread.SideTask.KeepAlive() 파일 C:\Users\user\Documents\Visual Studio 2015\Projects\prac_Thread\AbortingThread\MainApp.cs:줄 32
        Wating until thread stops
        Clearing resource
        Finished
    */
}
