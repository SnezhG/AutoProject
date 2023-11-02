using AutoService.Models;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IBusService
    {
        Task<IEnumerable<Bus>> GetBusesAsync();
        Task<Bus> GetBus(int id);
        Task<ServiceResponce> PutBus(int id, BusViewModel model);
        Task<ServiceResponce> PostBus(BusViewModel model);
        Task<ServiceResponce> DeleteBus(int id);
    }
}
