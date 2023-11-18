using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface ITripsService
    {
        Task<List<TripInfoDTO>> FindTrips(FindTripDTO model);
        Task<Trip> GetTrip(int id);
        Task<ServiceResponce> PutTrip(int id, TripDTO model);
        Task<ServiceResponce> PostTrip(TripDTO model);

    }
}
