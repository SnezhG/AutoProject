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
        
        public async Task<List<TripInfoDTO>> FindTrips([FromBody] FindTripDTO model)
        {
            var tripsFiltered = await _context.Trips
                .Include(trip => trip.Route)
                .Where(trip =>
                DateOnly.FromDateTime(trip.DepTime) == model.DepDate &&
                trip.Route.DepCity == model.DepCity &&
                trip.Route.ArrCity == model.ArrCity).ToListAsync();

            var tripsInfo = new List<TripInfoDTO>();
            
            foreach (var trip in tripsFiltered)
            {
                var tripInfo = new TripInfoDTO
                {
                    TripId = trip.TripId,
                    DepCity = trip.Route.DepCity,
                    ArrCity = trip.Route.ArrCity,
                    DepTime = trip.DepTime.ToShortTimeString(),
                    DepDate = DateOnly.FromDateTime(trip.DepTime),
                    ArrTime = trip.ArrTime.ToShortTimeString(),
                    ArrDate = DateOnly.FromDateTime(trip.ArrTime),
                    Price = trip.Price
                };
                tripsInfo.Add(tripInfo);
            }
            return tripsInfo;
        }
        
        public async Task<Trip> GetTrip(int id)
        {
            return await _context.Trips.FindAsync(id);
        }
        public async Task<ServiceResponce> PutTrip(int id, TripDTO model) 
        {
            var tripToEdit = await _context.Trips
                .Include(trip => trip.Bus)
                .Include(trip => trip.Driver)
                .Include(trip => trip.Conductor)
                .FirstOrDefaultAsync(trip => trip.TripId == id);

            if (tripToEdit == null)
                return new ServiceResponce 
                {
                    IsSuccess = false,
                    Message = "Trip not found"
                };

            if (tripToEdit.Bus.BusId != model.BusId)
            {
                var newBus = await _context.Buses.FindAsync(model.BusId);
                var oldBus = await _context.Buses.FindAsync(tripToEdit.Bus.BusId);
                if (newBus != null && oldBus != null)
                {
                    oldBus.Available = true;
                    newBus.Available = false;
                    _context.Buses.Update(oldBus);
                    _context.Buses.Update(newBus);
                }
                else
                {
                    return new ServiceResponce
                    {   
                        IsSuccess = false,
                        Message = "Buses not found"
                    };
                }
            }
            
            if (tripToEdit.DriverId != model.DriverId)
            {
                var newDriver = await _context.Personnel.FindAsync(model.DriverId);
                var oldDriver = await _context.Personnel.FindAsync(tripToEdit.DriverId);
                if (newDriver != null && oldDriver != null)
                {
                    oldDriver.Available = true;
                    newDriver.Available = false;
                    _context.Personnel.Update(oldDriver);
                    _context.Personnel.Update(newDriver); 
                }
                else
                {
                    return new ServiceResponce
                    {   
                        IsSuccess = false,
                        Message = "Drivers not found"
                    };
                }
            }
            
            if (tripToEdit.ConductorId != model.ConductorId)
            {
                var newCond = await _context.Personnel.FindAsync(model.ConductorId);
                var oldCond = await _context.Personnel.FindAsync(tripToEdit.ConductorId);
                if (newCond != null && oldCond != null)
                {
                    oldCond.Available = true;
                    newCond.Available = false;
                    _context.Personnel.Update(oldCond);
                    _context.Personnel.Update(newCond);
                }
                else
                {
                    return new ServiceResponce
                    {   
                        IsSuccess = false,
                        Message = "Conductors not found"
                    };
                }
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
                    IsSuccess = false,
                    Message = "Bus not found"
                };

            var driver = await _context.Personnel.FindAsync(model.DriverId);

            if (driver == null)
                return new ServiceResponce
                {
                    IsSuccess = false,
                    Message = "Driver not found"
                };

            var conductor = await _context.Personnel.FindAsync(model.ConductorId);

            if (conductor == null)
                return new ServiceResponce
                {
                    IsSuccess = false,
                    Message = "Conductor not found"
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
