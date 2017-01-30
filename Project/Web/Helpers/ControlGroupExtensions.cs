using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BootstrapSupport
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlGroup : IDisposable
    {
        private readonly HtmlHelper _html;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        public ControlGroup(HtmlHelper html){
            _html = html;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose(){
            _html.ViewContext.Writer.Write(_html.EndControlGroup());
        }
    }

    public static class ControlGroupExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="modelProperty"></param>
        /// <returns></returns>
        public static IHtmlString BeginControlGroupFor<T>(this HtmlHelper<T> html,Expression<Func<T, object>> modelProperty){
            return BeginControlGroupFor(html, modelProperty, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="modelProperty"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString BeginControlGroupFor<T>(this HtmlHelper<T> html,Expression<Func<T, object>> modelProperty,object htmlAttributes){
            return BeginControlGroupFor(html, modelProperty,
                                        HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="modelProperty"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString BeginControlGroupFor<T>(this HtmlHelper<T> html,Expression<Func<T, object>> modelProperty,IDictionary<string, object> htmlAttributes){
            var propertyName = ExpressionHelper.GetExpressionText(modelProperty);
            return BeginControlGroupFor(html, propertyName, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IHtmlString BeginControlGroupFor<T>(this HtmlHelper<T> html,string propertyName){
            return BeginControlGroupFor(html, propertyName, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="propertyName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString BeginControlGroupFor<T>(this HtmlHelper<T> html,string propertyName, object htmlAttributes){
            return BeginControlGroupFor(html, propertyName,HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="propertyName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString BeginControlGroupFor<T>(this HtmlHelper<T> html,string propertyName,IDictionary<string, object> htmlAttributes){
            var controlGroupWrapper = new TagBuilder("div");
            controlGroupWrapper.MergeAttributes(htmlAttributes);
            controlGroupWrapper.AddCssClass("control-group");
            var partialFieldName = propertyName;
            var fullHtmlFieldName =
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(partialFieldName);
            if (!html.ViewData.ModelState.IsValidField(fullHtmlFieldName)){
                controlGroupWrapper.AddCssClass("error");
            }
            var openingTag = controlGroupWrapper.ToString(TagRenderMode.StartTag);
            return MvcHtmlString.Create(openingTag);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IHtmlString EndControlGroup(this HtmlHelper html){
            return MvcHtmlString.Create("</div>");
        }

        public static ControlGroup ControlGroupFor<T>(this HtmlHelper<T> html,Expression<Func<T, object>> modelProperty){
            return ControlGroupFor(html, modelProperty, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="modelProperty"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static ControlGroup ControlGroupFor<T>(this HtmlHelper<T> html,Expression<Func<T, object>> modelProperty,object htmlAttributes){
            var propertyName = ExpressionHelper.GetExpressionText(modelProperty);
            return ControlGroupFor(html, propertyName,HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static ControlGroup ControlGroupFor<T>(this HtmlHelper<T> html, string propertyName){
            return ControlGroupFor(html, propertyName, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="propertyName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static ControlGroup ControlGroupFor<T>(this HtmlHelper<T> html, string propertyName,object htmlAttributes){
            return ControlGroupFor(html, propertyName,HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="propertyName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static ControlGroup ControlGroupFor<T>(this HtmlHelper<T> html, string propertyName,IDictionary<string, object> htmlAttributes){
            html.ViewContext.Writer.Write(BeginControlGroupFor(html, propertyName, htmlAttributes));
            return new ControlGroup(html);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class Alerts
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string Success = "success";
        /// <summary>
        /// 警告
        /// </summary>
        public const string Attention = "attention";
        /// <summary>
        /// 错误
        /// </summary>
        public const string Warning = "warning";
        /// <summary>
        /// 信息
        /// </summary>
        public const string Information = "info";

        public static string[] All => new[] { Success, Attention, Information, Warning };
    }
}
