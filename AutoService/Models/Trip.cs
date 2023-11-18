using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Trip
{
    public int TripId { get; set; }

    public DateTime DepTime { get; set; }

    public DateTime ArrTime { get; set; }

    public int BusId { get; set; }

    public int RouteId { get; set; }

    public int DriverId { get; set; }

    public int ConductorId { get; set; }

    public decimal Price { get; set; }

    public virtual Bus Bus { get; set; }

    public virtual Personnel Conductor { get; set; }

    public virtual Personnel Driver { get; set; }

    public virtual Busroute Route { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
