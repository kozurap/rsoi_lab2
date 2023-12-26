using Kernel.AbstractClasses;
using System;
using System.Collections.Generic;

namespace FlightService.Entities
{
    public partial class Airport : EntityBase
    {
        public Airport()
        {
            FlightFromairports = new HashSet<Flight>();
            FlightToairports = new HashSet<Flight>();
        }

        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public virtual ICollection<Flight> FlightFromairports { get; set; }
        public virtual ICollection<Flight> FlightToairports { get; set; }
    }
}
