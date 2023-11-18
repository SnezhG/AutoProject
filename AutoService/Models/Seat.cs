using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public sbyte Num { get; set; }

    public int BusId { get; set; }

    public bool Available { get; set; }

    public virtual Bus? Bus { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
