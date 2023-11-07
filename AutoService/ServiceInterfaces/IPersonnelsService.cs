using AutoService.Models;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.ServiceInterfaces
{
    public interface IPersonnelsService
    {
        Task<IEnumerable<Personnel>> GetPersonnels();
        Task<Personnel> GetPersonnel(int id);
        Task<ServiceResponce> PutPersonnel(int id, PersonnelDTO model);
        Task<ServiceResponce> PostPersonnel(PersonnelDTO model);
        Task<ServiceResponce> DeletePersonnel(int id);
    }
}
