using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Web.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class MikePagerExtensions
    {
        #region MikePager 分页控件

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MikePager(this AjaxHelper html, PagedResult data)
        {
            return MikePagerString(html.ViewContext, data.PageCount, data.CurrentPage, data.PageSize, data.RowCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MikePager(this HtmlHelper html, PagedResult data)
        {
            return MikePagerString(html.ViewContext, data.PageCount, data.CurrentPage, data.PageSize, data.RowCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="totalPage"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        private static string MikePagerString(ViewContext viewContext, int totalPage, int pageIndex, int pageSize, int totalCount)
        {
            var start = pageIndex - 5 >= 1 ? pageIndex - 5 : 1;
            var end = totalPage - start >= 10 ? start + 10 : totalPage;

            var url = new UrlHelper(viewContext.RequestContext);

            if (totalPage - pageIndex < 5)
            {
                start = totalPage - 10;
                if (start < 1)
                {
                    start = 1;
                }
            }

            var vs = viewContext.RouteData.Values;

            var queryString = viewContext.HttpContext.Request.QueryString;

            foreach (var key in queryString.AllKeys.Where(a => !a.IsEmpty()))
            {
                if (queryString[key] != "")
                {
                    vs[key] = queryString[key];
                }
            }
            
            vs.Remove("X-Requested-With");
            vs.Remove("X-HTTP-Method-Override");
            vs.Remove("_");

            var builder = new StringBuilder();
            builder.AppendFormat("<div class=\"pull-right\"><ul class=\"pagination\">");

            if (pageIndex > 1)
            {
                vs["pageIndex"] = 1;

                builder.Append($"<li><a href='#{url.Action("", vs)}'>|<</a></li>");

                vs["pageIndex"] = pageIndex - 1;

                builder.Append($"<li><a href='#{url.Action("", vs)}'><</a></li>");
            }

            for (var i = start; i <= end; i++) //前后各显示5个数字页码
            {
                vs["pageIndex"] = i;

                builder.Append(i == pageIndex ? "<li class=\"active\">" : "<li>");

                builder.Append($"<a href='#{url.Action("", vs)}'>{i}</a></li>");
            }

            if (pageIndex * pageSize < totalCount)
            {
                vs["pageIndex"] = pageIndex + 1;

                builder.Append($"<li><a href='#{url.Action("", vs)}'>></a></li>");

                vs["pageIndex"] = totalPage;

                builder.Append($"<li><a href='#{url.Action("", vs)}'>>|</a></li>");
            }

            builder.Append($"<li><span>每页{pageSize}条/共{totalCount}条 第{pageIndex}页/共{totalPage}页</span></li>");//可以改成从资源文件读取 适应多语言

            builder.Append("</ul>");

            builder.Append("</div>");

       
            return builder.ToString(); ;
        }

        #endregion
    }
}