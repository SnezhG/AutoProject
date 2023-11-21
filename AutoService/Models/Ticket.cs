using System;
using System.Collections.Generic;

namespace AutoService.Models;

public partial class Ticket
{
    public enum State
    {
        Issued,
        Booked,
        Paid,
        Cancelled,
        Expired
    }

    public enum Trigger
    {
        Book,
        Pay,
        Cancel,
        Expire
    }
    public int TicketId { get; set; }

    public int TripId { get; set; }

    public int PassengerId { get; set; }

    public int SeatId { get; set; }

    public DateTime DateTime { get; set; }

    public string Status { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual Trip Trip { get; set; } = null!;
    
    private State state = State.Issued;

    public State CurrentState
    {
        get { return state; }
    }

    public void TriggerState(Trigger trigger)
    {
        state = (state, trigger) switch
        {
            (State.Issued, Trigger.Book) => State.Booked,
            (State.Issued, Trigger.Pay) => State.Paid,
            (State.Booked, Trigger.Pay) => State.Paid,
            (State.Booked, Trigger.Cancel) => State.Cancelled,
            (State.Booked, Trigger.Expire) => State.Expired,
            _ => state
        };
    }
}
