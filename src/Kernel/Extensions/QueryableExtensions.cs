using System.Linq.Expressions;

namespace Kernel.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Appends expression to Queryable if (include == true) 
        /// </summary>
        /// <param name="q">Queryable that's going to be appended with exp</param>
        /// <param name="include">Bool which decides if to append exp or not</param>
        /// <param name="expression">Expression to be appended in success scenario</param>
        /// <typeparam name="T">Type of IQueryable</typeparam>
        /// <returns>IQueryable</returns>
        public static IQueryable<T> WhereNext<T>(this IQueryable<T> q, bool include,
            Expression<Func<T, bool>> expression)
            => include ? q.Where(expression) : q;

        /// <summary>
        /// Returns queryable from Queryable if (include == true) 
        /// </summary>
        /// <param name="q">Queryable that's going to be appended with exp</param>
        /// <param name="include">Bool which decides if to append exp or not</param>
        /// <param name="getter">Getter for queryable</param>
        /// <typeparam name="T">Type of IQueryable</typeparam>
        /// <returns>IQueryable</returns>
        public static IQueryable<T> WhereNext<T>(this IQueryable<T> q, bool include,
            Func<IQueryable<T>, IQueryable<T>> getter)
            => include ? getter(q) : q;

        /// <summary>
        /// Returns queryable from Queryable if (include != null) 
        /// </summary>
        /// <param name="q">Queryable that's going to be appended with exp</param>
        /// <param name="include">Bool which decides if to append exp or not</param>
        /// <param name="getter">Getter for queryable</param>
        /// <typeparam name="T">Type of IQueryable</typeparam>
        /// <returns>IQueryable</returns>
        public static IQueryable<T> WhereNext<T>(this IQueryable<T> q, object? include,
            Func<IQueryable<T>, IQueryable<T>> getter)
            => include != null ? getter(q) : q;

        /// <summary>
        /// Appends expression to Queryable if (include != null) 
        /// </summary>
        /// <param name="q">Queryable that's going to be appended with exp</param>
        /// <param name="include">Object which decides if to append exp or not</param>
        /// <param name="expression">Expression to be appended in success scenario</param>
        /// <typeparam name="T">Type of IQueryable</typeparam>
        /// <returns>IQueryable</returns>
        public static IQueryable<T> WhereNext<T>(this IQueryable<T> q, object? include,
            Expression<Func<T, bool>> expression)
            => include != null ? q.Where(expression) : q;
    }
}
