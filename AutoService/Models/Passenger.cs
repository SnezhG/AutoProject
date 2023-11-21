using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Passenger
{
    public int PassengerId { get; set; }

    public string? Surname { get; set; }

    public string Name { get; set; } = null!;

    public string Patronimyc { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string PassportNum { get; set; } = null!;

    public string PassportSeries { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
