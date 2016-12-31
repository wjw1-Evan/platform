using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Common
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Search<T>(this IQueryable<T> model, string keywords)
        {
            //Todo :让搜索支持多个关键字和日期字段搜索 "+"分隔
            if (!string.IsNullOrEmpty(keywords))
                foreach (var word in keywords.Split('+'))
                {
                    var keyword = word.Trim();
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        var where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(string)).Aggregate("1!=1 ", (current, item) => current + " or " + item.Name + ".Contains(@0)");

                        int intKeyword;
                        if (int.TryParse(keyword, out intKeyword))
                        {
                            where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(int)).Aggregate(where, (current, item) => current + " or " + item.Name + "==" + intKeyword);
                        }

                        decimal decimalKeyword;
                        if (decimal.TryParse(keyword, out decimalKeyword))
                        {
                            where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(decimal)).Aggregate(where, (current, item) => current + " or " + item.Name + "==" + decimalKeyword);
                        }

                        bool boolKeyword;
                        if (bool.TryParse(keyword, out boolKeyword))
                        {
                            where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(bool)).Aggregate(where, (current, item) => current + " or " + item.Name + "==" + boolKeyword);
                        }

                        ////支持搜索日期？
                        //DateTime dateKeyword;
                        //if (DateTime.TryParse(keyword, out dateKeyword))
                        //{
                        //    where = model.GetType().GetGenericArguments()[0].GetProperties().Where(item => item.PropertyType == typeof(DateTime)).Aggregate(where, (current, item) => current + " or (" + item.Name + "!=null And (DATEDIFF(" + item.Name + ",\""+ dateKeyword + "\")=0))");//(" + item.Name + "!=null And " + item.Name + ".Date.Equals(@0)");
                        //}

                        model = model.Where(where, keyword);
                    }
                }

            return model;
        }
    }

    public static class DistinctExtensions
    {
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }

        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector, comparer));
        }
    }

    // Distinct(p => p.Name, StringComparer.CurrentCultureIgnoreCase)

    public class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly Func<T, TV> _keySelector;
        private readonly IEqualityComparer<TV> _comparer;

        public CommonEqualityComparer(Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            _keySelector = keySelector;
            _comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TV> keySelector)
            : this(keySelector, EqualityComparer<TV>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }

}
