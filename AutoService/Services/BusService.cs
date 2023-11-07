using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AutoService.Services
{
    public class BusService : IBusService
    {
        private readonly AutoContext _context;

        public BusService(AutoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bus>> GetBusesAsync() 
        {
            return await _context.Buses.ToListAsync();
        }
        public async Task<Bus> GetBus(int id) 
        {
            return await _context.Buses.FindAsync(id);
        }
        public async Task<ServiceResponce> PutBus(int id, BusDTO model) 
        {
            var busToEdit = await _context.Buses.FindAsync(id);

            if (busToEdit == null)
                return new ServiceResponce
                {
                    IsSuccess = false
                };

            busToEdit.SeatCapacity = model.SeatCap;
            busToEdit.Available = model.Avail;
            busToEdit.Model = model.Model;
            busToEdit.Specs = model.Specs;

            _context.Buses.Update(busToEdit);

            await _context.SaveChangesAsync();

            return new ServiceResponce
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> PostBus(BusDTO model) 
        {
            var busToCreate = new Bus
            {
                SeatCapacity = model.SeatCap,
                Model = model.Model,
                Specs = model.Specs,
                Available = true
            };

            _context.Buses.Add(busToCreate);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> DeleteBus(int id) 
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
    }
}
