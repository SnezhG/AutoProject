using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IBusRoutesService
    {
        Task<IEnumerable<Busroute>> GetBusroutes();
        Task<Busroute> GetBusroute(int id);
        Task<ServiceResponce> PutBusroute(int id, BusRouteDTO model);
        Task<ServiceResponce> PostBusroute(BusRouteDTO model);
        Task<ServiceResponce> DeleteBusroute(int id);

    }
}
