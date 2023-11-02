using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Services
{
    public class BusRoutesService : IBusRoutesService
    {
        private readonly AutoContext _context;

        public BusRoutesService(AutoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Busroute>> GetBusroutes() 
        {
            return await _context.Busroutes.ToListAsync();
        }
        public async Task<Busroute> GetBusroute(int id) 
        {
            return await _context.Busroutes.FindAsync(id);
        }
        public async Task<ServiceResponce> PutBusroute(int id, BusRouteViewModel model) 
        {
            var routeToEdit = await _context.Busroutes.FindAsync(id);

            if (routeToEdit == null)
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            routeToEdit.ArrCity = model.ArrCity;
            routeToEdit.DepCity = model.DepCity;

            _context.Busroutes.Update(routeToEdit);

            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> PostBusroute(BusRouteViewModel model) 
        {
            var routeToCreate = new Busroute
            {
                ArrCity = model.ArrCity,
                DepCity = model.DepCity
            };

            _context.Busroutes.Add(routeToCreate);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> DeleteBusroute(int id) 
        {
            var routeToDelete = await _context.Busroutes.FindAsync(id);
            if (routeToDelete == null)
                return new ServiceResponce 
                {
                    IsSuccess = false
                };

            _context.Busroutes.Remove(routeToDelete);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }

    }
}
