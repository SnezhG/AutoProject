using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Passenger
{
    public int PassengerId { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronimyc { get; set; }

    public string? Sex { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? PassportNum { get; set; }

    public string? PassportSeries { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
