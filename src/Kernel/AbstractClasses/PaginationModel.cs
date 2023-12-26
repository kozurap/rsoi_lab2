using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.AbstractClasses
{
    public class PaginationModel<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalElements { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
