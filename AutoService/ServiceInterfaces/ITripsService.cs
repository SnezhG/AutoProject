using AutoService.Models;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface ITripsService
    {
        Task<IEnumerable<Trip>> FindTrips(FindTripViewModel model);
        Task<Trip> GetTrip(int id);
        Task<ServiceResponce> PutTrip(int id, TripViewModel model);
        Task<ServiceResponce> PostTrip(TripViewModel model);

    }
}
