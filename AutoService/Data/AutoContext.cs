using System;
using System.Collections.Generic;
using AutoService.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Data;

public partial class AutoContext : DbContext
{
    public AutoContext()
    {
    }

    public AutoContext(DbContextOptions<AutoContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseMySql(
                    "server=localhost;port=3306;database=autodb;uid=root;pwd=GIzy!", 
                    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));
        }
    }

    public virtual DbSet<Bus> Buses { get; set; }

    public virtual DbSet<Busroute> Busroutes { get; set; }

    public virtual DbSet<Clientticket> Clienttickets { get; set; }

    public virtual DbSet<Passenger> Passengers { get; set; }

    public virtual DbSet<Personnel> Personnel { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Bus>(entity =>
        {
            entity.HasKey(e => e.BusId).HasName("PRIMARY");

            entity.ToTable("bus");

            entity.Property(e => e.BusId).HasColumnName("busID");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.Model)
                .HasMaxLength(255)
                .HasColumnName("model");
            entity.Property(e => e.SeatCapacity).HasColumnName("seatCapacity");
            entity.Property(e => e.Specs)
                .HasMaxLength(255)
                .HasColumnName("specs");
        });

        modelBuilder.Entity<Busroute>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PRIMARY");

            entity.ToTable("busroute");

            entity.Property(e => e.RouteId).HasColumnName("routeId");
            entity.Property(e => e.ArrCity)
                .HasMaxLength(255)
                .HasColumnName("arrCity");
            entity.Property(e => e.DepCity)
                .HasMaxLength(255)
                .HasColumnName("depCity");
        });

        modelBuilder.Entity<Clientticket>(entity =>
        {
            entity.HasKey(e => e.TempId).HasName("PRIMARY");

            entity.ToTable("clienttickets");

            entity.Property(e => e.TempId).HasColumnName("tempId");
            entity.Property(e => e.Client)
                .HasMaxLength(255)
                .HasColumnName("client");
            entity.Property(e => e.Ticket).HasColumnName("ticket");
        });

        modelBuilder.Entity<Passenger>(entity =>
        {
            entity.HasKey(e => e.PassengerId).HasName("PRIMARY");

            entity.ToTable("passenger");

            entity.Property(e => e.PassengerId).HasColumnName("passengerId");
            entity.Property(e => e.DateOfBirth).HasColumnName("dateOfBirth");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PassportNum)
                .HasMaxLength(20)
                .HasColumnName("passportNum");
            entity.Property(e => e.PassportSeries)
                .HasMaxLength(10)
                .HasColumnName("passportSeries");
            entity.Property(e => e.Patronimyc)
                .HasMaxLength(255)
                .HasColumnName("patronimyc");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Sex)
                .HasColumnType("enum('female','male')")
                .HasColumnName("sex");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Personnel>(entity =>
        {
            entity.HasKey(e => e.PersonnelId).HasName("PRIMARY");

            entity.ToTable("personnel");

            entity.Property(e => e.PersonnelId).HasColumnName("personnelId");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Patronimyc)
                .HasMaxLength(255)
                .HasColumnName("patronimyc");
            entity.Property(e => e.Post)
                .HasColumnType("enum('conductor','driver')")
                .HasColumnName("post");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PRIMARY");

            entity.ToTable("seat");

            entity.HasIndex(e => e.BusId, "busId");

            entity.Property(e => e.SeatId).HasColumnName("seatId");
            entity.Property(e => e.Available).HasColumnName("available");
            entity.Property(e => e.BusId).HasColumnName("busId");
            entity.Property(e => e.Num).HasColumnName("num");

            entity.HasOne(d => d.Bus).WithMany(p => p.Seats)
                .HasForeignKey(d => d.BusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("seat_ibfk_1");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PRIMARY");

            entity.ToTable("ticket");

            entity.HasIndex(e => e.PassengerId, "passengerId");

            entity.HasIndex(e => e.SeatId, "seatId");

            entity.HasIndex(e => e.TripId, "tripId");

            entity.Property(e => e.TicketId).HasColumnName("ticketId");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("dateTime");
            entity.Property(e => e.PassengerId).HasColumnName("passengerId");
            entity.Property(e => e.SeatId).HasColumnName("seatId");
            entity.Property(e => e.Status)
                .HasColumnType("enum('issued','booked','paid','cancelled','expired')")
                .HasColumnName("status");
            entity.Property(e => e.TripId).HasColumnName("tripId");

            entity.HasOne(d => d.Passenger).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.PassengerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ticket_ibfk_2");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ticket_ibfk_3");

            entity.HasOne(d => d.Trip).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TripId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ticket_ibfk_1");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PRIMARY");

            entity.ToTable("trip");

            entity.HasIndex(e => e.BusId, "busId");

            entity.HasIndex(e => e.ConductorId, "conductorId");

            entity.HasIndex(e => e.DriverId, "driverId");

            entity.HasIndex(e => e.RouteId, "routeId");

            entity.Property(e => e.TripId).HasColumnName("tripId");
            entity.Property(e => e.ArrTime)
                .HasColumnType("datetime")
                .HasColumnName("arrTime");
            entity.Property(e => e.BusId).HasColumnName("busId");
            entity.Property(e => e.ConductorId).HasColumnName("conductorId");
            entity.Property(e => e.DepTime)
                .HasColumnType("datetime")
                .HasColumnName("depTime");
            entity.Property(e => e.DriverId).HasColumnName("driverId");
            entity.Property(e => e.Price)
                .HasPrecision(5)
                .HasColumnName("price");
            entity.Property(e => e.RouteId).HasColumnName("routeId");

            entity.HasOne(d => d.Bus).WithMany(p => p.Trips)
                .HasForeignKey(d => d.BusId)
                .HasConstraintName("trip_ibfk_1");

            entity.HasOne(d => d.Conductor).WithMany(p => p.TripConductors)
                .HasForeignKey(d => d.ConductorId)
                .HasConstraintName("trip_ibfk_4");

            entity.HasOne(d => d.Driver).WithMany(p => p.TripDrivers)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("trip_ibfk_3");

            entity.HasOne(d => d.Route).WithMany(p => p.Trips)
                .HasForeignKey(d => d.RouteId)
                .HasConstraintName("trip_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
