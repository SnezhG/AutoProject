using AutoService.Models;
using AutoService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IPersonnelsService
    {
        Task<IEnumerable<Personnel>> GetPersonnels();
        Task<Personnel> GetPersonnel(int id);
        Task<ServiceResponce> PutPersonnel(int id, PersonnelViewModel model);
        Task<ServiceResponce> PostPersonnel(PersonnelViewModel model);
        Task<ServiceResponce> DeletePersonnel(int id);
    }
}
