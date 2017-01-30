using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using EntityFramework.Extensions;
using IServices.ISysServices;
using Models.Infrastructure;

namespace Web.Helpers
{
    public static class HtmlHelperExtensions
    {

        /// <summary>
        /// 将数据变成 无限分类下拉框
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">需要实现 IUserDictionary</param>
        /// <param name="id">选中值的ID</param>
        /// <returns></returns>
        public static IList<IUserDictionary> ToSystemIdSelectList<T>(this IEnumerable<T> model, string id)
        {
            var queryable = model as IEnumerable<IUserDictionary>;

            if (queryable == null) return null;

            var systemIdSelectList = queryable as IList<IUserDictionary> ?? queryable.ToList();

            foreach (var item in systemIdSelectList.Where(a=>a.Enable))
            {
                item.Selected = item.Id == id;
            }

            return systemIdSelectList;
        }


        public static RouteValueDictionary ToRouteValueDictionary(this HttpRequestBase model)
        {
            var routevalues = new RouteValueDictionary();

            foreach (var key in model.QueryString.AllKeys.Where(a => !a.IsEmpty()))
            {
                routevalues.Add(key, model.QueryString[key]);
            }

            routevalues.Remove("X-Requested-With");
            routevalues.Remove("X-HTTP-Method-Override");
            routevalues.Remove("_");

            return routevalues;
        }

        public static bool CheckControllerAction(this HtmlHelper html, string action)
        {
            var area = (string)html.ViewContext.RouteData.DataTokens["area"];

            var controller = (string)html.ViewContext.RouteData.Values["controller"];

            var sysRoleService = DependencyResolver.Current.GetService<ISysRoleService>();

            var userInfo = DependencyResolver.Current.GetService<IUserInfo>();

            return sysRoleService.CheckSysUserSysRoleSysControllerSysActions(userInfo.UserId, area, action,controller);
        }

        public static string IsActive(this HtmlHelper html, string controller = null, string action = null)
        {
            var activeClass = "active"; // change here if you another name to activate sidebar items
            // detect current app state
            var actualAction = (string)html.ViewContext.RouteData.Values["action"];
            var actualController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = actualController;

            if (String.IsNullOrEmpty(action))
                action = actualAction;

            return controller == actualController && action == actualAction ? activeClass : String.Empty;
        }

        public static string ThisControllerName(this HtmlHelper html)
        {

            var iSysControllerService = DependencyResolver.Current.GetService<ISysControllerService>();

            var area = (string)HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];

            var controller = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];

            var item = iSysControllerService.GetAll(a => a.ControllerName == controller && a.SysArea.AreaName == area).FromCacheFirstOrDefault();

            return item?.Name;
        }

        /// <summary>
        /// 去除html代码
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(this string html, int length = 0)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }

            var strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");

            if (string.IsNullOrEmpty(strText))
            {
                return "";
            }

            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            return length > 0 ? strText.CutString(length) : strText;
        }

        /// <summary>
        /// 截取指定长度的字符串
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="len">要截取的长度</param>
        /// <param name="flag">截取后是否加省略号(true加,false不加)</param>
        /// <returns></returns>
        public static string CutString(this string str, int len, bool flag = true)
        {
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return "";
            }

            var _outString = "";
            var _len = 0;
            for (var i = 0; i < str.Length; i++)
            {
                if (Char.ConvertToUtf32(str, i) >= Convert.ToInt32("4e00", 16) &&
                    Char.ConvertToUtf32(str, i) <= Convert.ToInt32("9fff", 16))
                {
                    _len += 2;
                    if (_len > len) //截取的长度若是最后一个占两个字节，则不截取
                    {
                        break;
                    }
                }
                else
                {
                    _len++;
                }


                try
                {
                    _outString += str.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (_len >= len)
                {
                    break;
                }
            }
            if (str != _outString && flag == true) //判断是否添加省略号
            {
                _outString += "...";
            }
            return _outString;
        }

        /// <summary>
        /// 处理时间显示为距离现在多久
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DateStringFromNow(this DateTime dt)
        {
            var span = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "China Standard Time") - dt;

            if (span.TotalDays > 1)
            {
                return dt.ToShortDateString();
            }
            else
            {
                if (span.TotalHours > 1)
                {
                    return
                        $"{(int)Math.Floor(span.TotalHours)}小时前";
                }
                else
                {
                    if (span.TotalMinutes > 1)
                    {
                        return
                            $"{(int)Math.Floor(span.TotalMinutes)}分钟前";
                    }
                    else
                    {
                        if (span.TotalSeconds >= 0)
                        {
                            return
                                $"{(int)Math.Floor(span.TotalSeconds)}秒前";
                        }
                        else
                        {
                            return dt.ToShortDateString();
                        }
                    }

                }
            }
        }
    }
}
