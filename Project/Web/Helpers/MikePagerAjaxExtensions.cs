using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.WebPages;

namespace Web.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class MikePagerAjaxExtensions
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
        public static string MikePager<T>(this AjaxHelper html, PagedList<T> data, string updateTargetId = "Main") where T : class
        {
            return html.MikePager(data.PageIndex, data.PageSize, data.TotalCount, updateTargetId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="updateTargetId"></param>
        /// <returns></returns>
        public static string MikePager(this AjaxHelper html, int pageIndex, int pageSize, int totalCount,string updateTargetId="Main")
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
            foreach (var key in queryString.AllKeys.Where(a=> !a.IsEmpty()))
                vs[key] = queryString[key];

            var formString = html.ViewContext.HttpContext.Request.Form;
            foreach (var key in formString.AllKeys.Where(a => !a.IsEmpty()))
                vs[key] = formString[key];

            vs.Remove("X-Requested-With");
            vs.Remove("X-HTTP-Method-Override");

            var builder = new StringBuilder();
            builder.AppendFormat("<div class=\"pull-right wfom4M\"><ul class=\"pagination\">");

            //vs["pageSize"] = data.PageSize;
            if (pageIndex > 1)
            {
                vs["pageIndex"] = 1;

                builder.Append("<li>");
                builder.Append(html.ActionLink("|<", vs["action"].ToString(), vs,
                    new AjaxOptions { UpdateTargetId = updateTargetId }));
                builder.Append("</li>");

                vs["pageIndex"] = pageIndex - 1;
                builder.Append("<li class=\"ui-state-default  ui-corner-all\">");
                builder.Append(html.ActionLink("<", vs["action"].ToString(), vs,
                    new AjaxOptions { UpdateTargetId = updateTargetId }));
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
                    builder.Append(html.ActionLink(i.ToString(CultureInfo.InvariantCulture), vs["action"].ToString(), vs,
                        new AjaxOptions { UpdateTargetId = updateTargetId }));
                    builder.Append("</li>");
                }
            }

            if (pageIndex * pageSize < totalCount)
            {
                vs["pageIndex"] = pageIndex + 1;
                builder.Append("<li>");
                builder.Append(html.ActionLink(">", vs["action"].ToString(), vs,
                    new AjaxOptions { UpdateTargetId = updateTargetId }));
                builder.Append("</li>");

                vs["pageIndex"] = totalPage;
                builder.Append("<li>");
                builder.Append(html.ActionLink(">|", vs["action"].ToString(), vs,
                    new AjaxOptions { UpdateTargetId = updateTargetId }));
                builder.Append("</li>");
            }
            builder.Append("<li>");


            var url = new UrlHelper(html.ViewContext.RequestContext);
            vs.Remove("pageIndex");
            builder.Append("<span>");
            builder.Append("<form class=\"fom4M\" action=\"" + url.Action(vs["action"].ToString(), vs) +
                           "\" data-ajax=\"true\"  data-ajax-mode=\"replace\" data-ajax-update=\"#" + updateTargetId + "\" id=\"form1\" method=\"post\" >");
            builder.Append("每页" + pageSize + "条/共" + totalCount + "条 第");
            builder.Append("<input type=\"text\" id=\"pageIndex\" name=\"pageIndex\" size=4  style=\"border:0;border-bottom: 1px solid white;text-align: center;padding:0;  background-color:transparent;\" value=" + pageIndex + " />");
            builder.Append("页/共" + totalPage + "页");
            builder.Append("</form>");

            builder.Append("</span>");
            builder.Append("</li></ul>");
            builder.Append("</div>");
            return builder.ToString();
        }

        #endregion
    }
}