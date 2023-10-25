using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Bus
{
    public int BusId { get; set; }

    public sbyte? SeatCapacity { get; set; }

    public string? Model { get; set; }

    public string? Specs { get; set; }

    public bool? Available { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
