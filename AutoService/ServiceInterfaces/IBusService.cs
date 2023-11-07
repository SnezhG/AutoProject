using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IBusService
    {
        Task<IEnumerable<Bus>> GetBusesAsync();
        Task<Bus> GetBus(int id);
        Task<ServiceResponce> PutBus(int id, BusDTO model);
        Task<ServiceResponce> PostBus(BusDTO model);
        Task<ServiceResponce> DeleteBus(int id);
    }
}
