using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Busroute
{
    public int RouteId { get; set; }

    public string? DepCity { get; set; }

    public string? ArrCity { get; set; }

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
