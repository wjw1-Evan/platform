using System;
using System.Linq;
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
        /// <param name="updateTargetId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string MikePager(this AjaxHelper html, IPagedList data, string updateTargetId = "Main")
        {
            return MikePagerString(html.ViewContext, data.PageIndex, data.PageSize, data.TotalCount, updateTargetId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string MikePager(this HtmlHelper html, IPagedList data)
        {
            return MikePagerString(html.ViewContext, data.PageIndex, data.PageSize, data.TotalCount, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewContext"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="updateTargetId"></param>
        /// <returns></returns>
        private static string MikePagerString(ViewContext viewContext, int pageIndex, int pageSize, int totalCount, string updateTargetId)
        {
            var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
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

            vs["updateTargetId"] = updateTargetId;

            var queryString = viewContext.HttpContext.Request.QueryString;

            foreach (var key in queryString.AllKeys.Where(a => !a.IsEmpty()))
            {
                if (queryString[key] != "")
                {
                    vs[key] = queryString[key];
                }
            }

            var formString = viewContext.HttpContext.Request.Form;

            foreach (var key in formString.AllKeys.Where(a => !a.IsEmpty()))
            {
                if (formString[key] != "")
                {
                    vs[key] = formString[key];
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

                builder.Append($"<li><a href='#{url.Action(vs["action"].ToString(), vs)}'>|<</a></li>");

                vs["pageIndex"] = pageIndex - 1;

                builder.Append($"<li><a href='#{url.Action(vs["action"].ToString(), vs)}'><</a></li>");
            }

            for (var i = start; i <= end; i++) //前后各显示5个数字页码
            {
                vs["pageIndex"] = i;

                builder.Append(i == pageIndex ? "<li class=\"active\">" : "<li>");

                builder.Append($"<a href='#{url.Action(vs["action"].ToString(), vs)}'>{i}</a></li>");
            }

            if (pageIndex * pageSize < totalCount)
            {
                vs["pageIndex"] = pageIndex + 1;

                builder.Append($"<li><a href='#{url.Action(vs["action"].ToString(), vs)}'>></a></li>");
           
                vs["pageIndex"] = totalPage;

                builder.Append($"<li><a href='#{url.Action(vs["action"].ToString(), vs)}'>>|</a></li>");
            }
 
            builder.Append($"<li><span>每页{pageSize}条/共{totalCount}条 第{pageIndex}页/共{totalPage}页</span></li>");//可以改成从资源文件读取 适应多语言

            builder.Append("</ul>");

            builder.Append("</div>");

            var buliderstring = builder.ToString();


            if (string.IsNullOrEmpty(updateTargetId))
            {
                buliderstring = buliderstring.Replace("#", "");
            }

            return buliderstring;
        }

        #endregion
    }
}