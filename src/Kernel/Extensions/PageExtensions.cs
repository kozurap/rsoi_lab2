using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Extensions
{
    public static class PageExtensions
    {
        public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> source, int page, int pageSize) => source.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
