using AutoService.Models;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IBusRoutesService
    {
        Task<IEnumerable<Busroute>> GetBusroutes();
        Task<Busroute> GetBusroute(int id);
        Task<ServiceResponce> PutBusroute(int id, BusRouteViewModel model);
        Task<ServiceResponce> PostBusroute(BusRouteViewModel model);
        Task<ServiceResponce> DeleteBusroute(int id);

    }
}
