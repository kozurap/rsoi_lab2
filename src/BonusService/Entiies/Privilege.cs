using Kernel.AbstractClasses;
using System;
using System.Collections.Generic;

namespace PrivilegeService.Entiies
{
    public partial class Privilege : EntityBase
    {
        public Privilege()
        {
            PrivilegeHistories = new HashSet<PrivilegeHistory>();
        }

        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? Balance { get; set; }

        public virtual ICollection<PrivilegeHistory> PrivilegeHistories { get; set; }
    }
}
