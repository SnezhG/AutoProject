using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Clientticket
{
    public int TempId { get; set; }

    public int Ticket { get; set; }

    public string Client { get; set; }
}
