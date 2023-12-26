using Kernel.AbstractClasses;
using System;
using System.Collections.Generic;

namespace PrivilegeService.Entiies
{
    public partial class PrivilegeHistory : EntityBase
    {
        public int? PrivilegeId { get; set; }
        public Guid TicketUid { get; set; }
        public DateTime Datetime { get; set; }
        public int BalanceDiff { get; set; }
        public string OperationType { get; set; } = null!;

        public virtual Privilege? Privilege { get; set; }
    }
}
