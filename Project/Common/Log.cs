using System;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetInnerException(this Exception ex)
        {
            // 递归
            // return ex.InnerException != null ? GetInnerException(ex.InnerException) : ex.Message;

            // 循环
            var str = ex.Message;
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                str = ex.ToString();   // ex.Message;
            }
            return str;
        }
    }
}