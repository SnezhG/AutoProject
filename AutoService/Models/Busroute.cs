using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Busroute
{
    public int RouteId { get; set; }

    public string DepCity { get; set; } = null!;

    public string ArrCity { get; set; } = null!;

    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
}
