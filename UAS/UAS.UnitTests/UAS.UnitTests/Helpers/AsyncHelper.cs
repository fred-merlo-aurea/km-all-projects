using System.Threading;

namespace UAS.UnitTests.Helpers
{
    public static class AsyncHelper
    {
        /// <summary>
        /// Waits for async operation
        /// </summary>
        /// <param name="ms">Time to wait in miliseconds</param>
        public static void Wait(int ms)
        {
            ManualResetEvent done = new ManualResetEvent(false);
            Thread thread = new Thread(delegate () {
                Thread.Sleep(ms);
                done.Set();
            });
            thread.Start();

            done.WaitOne();
            done.Dispose();
        }
    }
}
