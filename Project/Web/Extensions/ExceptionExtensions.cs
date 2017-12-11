using System;


namespace Web.Extensions
{
    /// <summary>
    /// 异常信息相关扩展
    /// </summary>
    public static class ExceptionExtensions
    {
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