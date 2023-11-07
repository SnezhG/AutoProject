﻿using System;
using System.Collections.Generic;
using Quartz;

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

    public int? TripId { get; set; }

    public int? PassengerId { get; set; }

    public int? SeatId { get; set; }

    public DateTime DateTime { get; set; }

    public string? Status { get; set; }

    public virtual Passenger? Passenger { get; set; }

    public virtual Seat? Seat { get; set; }

    public virtual Trip? Trip { get; set; }

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
