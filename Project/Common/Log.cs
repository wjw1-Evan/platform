using System.Diagnostics;

namespace Common
{
    public static class Log
    {
        public static void Write(string category, string message)
        {
            //输出到 Trace.axd
            Trace.WriteLine(message, category);
        }

    }
}