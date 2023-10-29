using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? TripId { get; set; }

    public int? PassengerId { get; set; }

    public int? SeatId { get; set; }

    public DateTime DateTime { get; set; }

    public string? Status { get; set; }

    public virtual Passenger? Passenger { get; set; }

    public virtual Seat? Seat { get; set; }

    public virtual Trip? Trip { get; set; }
}
