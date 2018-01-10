using EFSecondLevelCache;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Web.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// 
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int PageSize { get; set; }

    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList where T : class
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            var data = source.Skip((index - 1) * pageSize).Take(pageSize).Cacheable().ToListAsync();
            TotalCount = source.Cacheable().CountAsync().Result;
            PageSize = pageSize;
            PageIndex = index;
            if (TotalCount / pageSize < PageIndex - 1)
            {
                index = 1;
                PageIndex = index;
            }
            AddRange(data.Result);
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public int TotalCount { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public int PageIndex { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public int PageSize { get; set; }


    }


    /// <summary>
    /// 
    /// </summary>
    public static class Pagination
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index = 1, int pageSize = 20) where T : class
        {
            return new PagedList<T>(source, index, pageSize);
        }
    }
}
