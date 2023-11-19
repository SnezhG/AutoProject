using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Personnel
{
    public int PersonnelId { get; set; }

    public string Surname { get; set; }

    public string Name { get; set; }

    public string? Patronimyc { get; set; }

    public string Post { get; set; }

    public sbyte Experience { get; set; }

    public bool Available { get; set; }

    public virtual ICollection<Trip> TripConductors { get; set; } = new List<Trip>();

    public virtual ICollection<Trip> TripDrivers { get; set; } = new List<Trip>();
}
