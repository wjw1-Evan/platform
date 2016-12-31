using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Web.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class MikePagerHtmlExtensions
    {
        #region MikePager 分页控件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string MikePager<T>(this HtmlHelper html, PagedList<T> data) where T : class
        {
            return html.MikePager(data.PageIndex, data.PageSize, data.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static string MikePager(this HtmlHelper html, int pageIndex, int pageSize, int totalCount)
        {
            var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
            var start = pageIndex - 5 >= 1 ? pageIndex - 5 : 1;
            var end = totalPage - start >= 10 ? start + 10 : totalPage;

            if (totalPage - pageIndex < 5)
            {
                start = totalPage - 10;
                if (start < 1)
                {
                    start = 1;
                }
            }

            var vs = html.ViewContext.RouteData.Values;

            var queryString = html.ViewContext.HttpContext.Request.QueryString;
            foreach (var key in queryString.AllKeys)
                vs[key] = queryString[key];

            var formString = html.ViewContext.HttpContext.Request.Form;
            foreach (var key in formString.AllKeys)
                vs[key] = formString[key];

            vs.Remove("X-Requested-With");
            vs.Remove("X-HTTP-Method-Override");

            var builder = new StringBuilder();
            builder.AppendFormat("<div><ul class=\"pagination\">");

            //vs["pageSize"] = data.PageSize;
            if (pageIndex > 1)
            {
                vs["pageIndex"] = 1;

                builder.Append("<li>");
                builder.Append(html.ActionLink("|<", vs["action"].ToString(), vs));
                builder.Append("</li>");

                vs["pageIndex"] = pageIndex - 1;
                builder.Append("<li class=\"ui-state-default  ui-corner-all\">");
                builder.Append(html.ActionLink("<", vs["action"].ToString(), vs));
                builder.Append("</li>");
            }

            for (var i = start; i <= end; i++) //前后各显示5个数字页码
            {
                vs["pageIndex"] = i;

                if (i == pageIndex)
                {
                    builder.Append("<li class=\"active\"><a href=\"#\">");
                    builder.Append(i);
                    builder.Append("</a></li>");
                }
                else
                {
                    builder.Append("<li>");
                    builder.Append(html.ActionLink(i.ToString(CultureInfo.InvariantCulture), vs["action"].ToString(), vs));
                    builder.Append("</li>");
                }
            }

            if (pageIndex * pageSize < totalCount)
            {
                vs["pageIndex"] = pageIndex + 1;
                builder.Append("<li>");
                builder.Append(html.ActionLink(">", vs["action"].ToString(), vs));
                builder.Append("</li>");

                vs["pageIndex"] = totalPage;
                builder.Append("<li>");
                builder.Append(html.ActionLink(">|", vs["action"].ToString(), vs));
                builder.Append("</li>");
            }

            builder.Append("</li></ul>");
            builder.Append("</div>");
            return builder.ToString();
        }

        #endregion
    }
}