using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IBusService
    {
        Task<IEnumerable<Bus>> GetBusesAsync();
        Task<Bus> GetBus(int id);
        Task<IEnumerable<Seat>> GetBusSeats(int busId);
        Task<ServiceResponce> PutBus(int id, BusDTO model);
        Task<ServiceResponce> PostBus(BusDTO model);
        Task<ServiceResponce> DeleteBus(int id);
    }
}
