using Kernel.AbstractClasses;
using System;
using System.Collections.Generic;

namespace TicketService.Entities
{
    public partial class Ticket : EntityBase
    {
        public Guid Ticketuid { get; set; }
        public string Username { get; set; } = null!;
        public string Flightnumber { get; set; } = null!;
        public int Price { get; set; }
        public string Status { get; set; } = null!;
    }
}
