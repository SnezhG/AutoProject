using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Services
{
    public class TripsService : ITripsService
    {
        private readonly AutoContext _context;

        public TripsService(AutoContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Trip>> FindTrips([FromBody] FindTripDTO model) 
        {
            return await _context.Trips.Where(trip =>
                    DateOnly.FromDateTime(trip.DepTime) == model.DepDate &&
                    trip.Route.DepCity == model.DepCity &&
                    trip.Route.ArrCity == model.ArrCity).ToListAsync();

        }
        public async Task<Trip> GetTrip(int id)
        {
            return await _context.Trips.FindAsync(id);
        }
        public async Task<ServiceResponce> PutTrip(int id, TripDTO model) 
        {
            var tripToEdit = await _context.Trips.FindAsync(id);

            if (tripToEdit == null)
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            if (tripToEdit.Bus.BusId != model.BusId)
            {
                var newBus = await _context.Buses.FindAsync(model.BusId);
                var oldBus = await _context.Buses.FindAsync(tripToEdit.Bus.BusId);
                oldBus.Available = true;
                newBus.Available = false;
                _context.Buses.Update(oldBus);
                _context.Buses.Update(newBus);
            }
            
            if (tripToEdit.DriverId != model.DriverId)
            {
                var newDriver = await _context.Personnel.FindAsync(model.DriverId);
                var oldDriver = await _context.Personnel.FindAsync(tripToEdit.DriverId);
                oldDriver.Available = true;
                newDriver.Available = false;
                _context.Personnel.Update(oldDriver);
                _context.Personnel.Update(newDriver);
            }
            
            if (tripToEdit.ConductorId != model.ConductorId)
            {
                var newCond = await _context.Personnel.FindAsync(model.ConductorId);
                var oldCond = await _context.Personnel.FindAsync(tripToEdit.ConductorId);
                oldCond.Available = true;
                newCond.Available = false;
                _context.Personnel.Update(oldCond);
                _context.Personnel.Update(newCond);
            }

            tripToEdit.DepTime = model.DepTime;
            tripToEdit.ArrTime = model.ArrTime;
            tripToEdit.BusId = model.BusId;
            tripToEdit.RouteId = model.RouteId;
            tripToEdit.DriverId = model.DriverId;
            tripToEdit.ConductorId = model.ConductorId;
            tripToEdit.Price = model.Price;

            _context.Trips.Update(tripToEdit);
            
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> PostTrip(TripDTO model) 
        {
            var tripToCreate = new Trip
            {
                DepTime = model.DepTime,
                ArrTime = model.ArrTime,
                BusId = model.BusId,
                RouteId = model.RouteId,
                DriverId = model.DriverId,
                ConductorId = model.ConductorId,
                Price = model.Price
            };

            var bus = await _context.Buses.FindAsync(model.BusId);

            if (bus == null)
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            var driver = await _context.Personnel.FindAsync(model.DriverId);

            if (driver == null)
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            var conductor = await _context.Personnel.FindAsync(model.ConductorId);

            if (conductor == null)
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            bus.Available = false;
            driver.Available = false;
            conductor.Available = false;

            _context.Buses.Update(bus);
            _context.Personnel.Update(driver);
            _context.Personnel.Update(conductor);

            _context.Trips.Add(tripToCreate);

            await _context.SaveChangesAsync();

            return new ServiceResponce
            {
                IsSuccess = true
            };
        }
    }
}
