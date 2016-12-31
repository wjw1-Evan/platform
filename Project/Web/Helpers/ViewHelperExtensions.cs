using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;

namespace BootstrapSupport
{
    /// <summary>
    /// 
    /// </summary>
    public static class DefaultScaffoldingExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public static string GetControllerName(this Type controllerType)
        {
            return controllerType.Name.Replace("Controller", String.Empty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExpression"></param>
        /// <returns></returns>
        public static string GetActionName(this LambdaExpression actionExpression)
        {
            return ((MethodCallExpression)actionExpression.Body).Method.Name;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static PropertyInfo[] VisibleProperties(this IEnumerable Model)
        {
            var elementType = Model.GetType().GetElementType();
            if (elementType == null)
            {
                elementType = Model.GetType().GetGenericArguments()[0];
            }
            //return elementType.GetProperties().Where(info => info.Name != elementType.IdentifierPropertyName()).OrderedByDisplayAttr().ToArray();
            return elementType.GetProperties().OrderedByDisplayAttr().ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static PropertyInfo[] VisibleProperties(this Object model)
        {
            //return model.GetType().GetProperties().Where(info => info.Name != model.IdentifierPropertyName()).OrderedByDisplayAttr().ToArray();
            return model.GetType().GetProperties().OrderedByDisplayAttr().ToArray();
        }

        // Support for Order property in DisplayAttribute ([Display(..., Order = n)])
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<PropertyInfo> OrderedByDisplayAttr(this IEnumerable<PropertyInfo> collection)
        {
            return collection.OrderBy(col =>
            {
                var attr = col.GetAttribute<DisplayAttribute>();
                return (attr != null ? attr.GetOrder() : null) ?? 0;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetIdValue(this object model)
        {
            var v = new RouteValueDictionary();
            v.Add(model.IdentifierPropertyName(), model.GetId());
            return v;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static object GetId(this object model)
        {
            return model.GetType().GetProperty(model.IdentifierPropertyName()).GetValue(model,new object[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string IdentifierPropertyName(this Object model)
        {
            return IdentifierPropertyName(model.GetType());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string IdentifierPropertyName(this Type type)
        {
            if (type.GetProperties().Any(info => info.AttributeExists<KeyAttribute>()))
            {
                return
                    type.GetProperties().First(
                        info => info.AttributeExists<KeyAttribute>())
                        .Name;
            }
            else if (type.GetProperties().Any(p => p.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)))
            {
                return
                    type.GetProperties().First(
                        p => p.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)).Name;
            }
            else if (type.GetProperties().Any(p => p.Name.Equals(type.Name + "id", StringComparison.CurrentCultureIgnoreCase)))
            {
                return
                    type.GetProperties().First(
                        p => p.Name.Equals(type.Name + "id", StringComparison.CurrentCultureIgnoreCase)).Name;
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetLabel(this PropertyInfo propertyInfo)
        {
            var meta = ModelMetadataProviders.Current.GetMetadataForProperty(null, propertyInfo.DeclaringType, propertyInfo.Name);
            return meta.GetDisplayName();
        }

        public static string ToSeparatedWords(this string value)
        {
            return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : class
        {
            var attribute = propertyInfo.GetCustomAttributes(typeof(T), false)
                                .FirstOrDefault() as T;
            if (attribute == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool AttributeExists<T>(this Type type) where T : class
        {
            var attribute = type.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
            if (attribute == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : class
        {
            return type.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : class
        {
            return propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
		/// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LabelFromType(Type @type)
        {
            var att = GetAttribute<DisplayNameAttribute>(@type);
            return att != null ? att.DisplayName 
                : @type.Name.ToSeparatedWords();
        }
		/// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static string GetLabel(this Object Model)
        {
            return LabelFromType(Model.GetType());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static string GetLabel(this IEnumerable Model)
        {
            var elementType = Model.GetType().GetElementType() ?? Model.GetType().GetGenericArguments()[0];
            return LabelFromType(elementType);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString TryPartial(this HtmlHelper helper, string viewName, object model)
        {
            try
            {
                return helper.Partial(viewName, model);
            }
            catch (Exception)
            {
            }
            return MvcHtmlString.Empty;
        }
    }
}