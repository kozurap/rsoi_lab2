using Kernel.AbstractClasses;
using System;
using System.Collections.Generic;

namespace FlightService.Entities
{
    public partial class Flight : EntityBase
    {
        public string Flightnumber { get; set; } = null!;
        public DateTime Datetime { get; set; }
        public int? Fromairportid { get; set; }
        public int? Toairportid { get; set; }
        public int Price { get; set; }

        public virtual Airport? Fromairport { get; set; }
        public virtual Airport? Toairport { get; set; }
    }
}
