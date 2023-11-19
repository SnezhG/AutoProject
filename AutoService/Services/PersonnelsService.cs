using AutoService.Data;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Services
{
    public class PersonnelsService : IPersonnelsService
    {
        private readonly AutoContext _context;

        public PersonnelsService(AutoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Personnel>> GetPersonnels() 
        {
            return await _context.Personnel.ToListAsync();
        }
        public async Task<Personnel> GetPersonnel(int id) 
        {
            return await _context.Personnel.FindAsync(id);
        }
        public async Task<ServiceResponce> PutPersonnel(int id, PersonnelDTO model) 
        {
            var persToEdit = await _context.Personnel.FindAsync(id);

            if (persToEdit == null)
                return new ServiceResponce 
                {
                    IsSuccess = false,
                    Message = "Personnel not found"
                };

            persToEdit.Surname = model.Surname;
            persToEdit.Name = model.Name;
            persToEdit.Patronimyc = model.Patronimyc;
            persToEdit.Post = model.Post;
            persToEdit.Experience = model.Experience;

            _context.Personnel.Update(persToEdit);

            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> PostPersonnel(PersonnelDTO model) 
        {
            var persToCreate = new Personnel
            {
                Surname = model.Surname,
                Name = model.Name,
                Patronimyc = model.Patronimyc,
                Post = model.Post,
                Experience = model.Experience,
                Available = true
            };

            _context.Personnel.Add(persToCreate);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResponce> DeletePersonnel(int id) 
        {
            var persToDelete = await _context.Personnel.FindAsync(id);
            if (persToDelete == null)
                return new ServiceResponce 
                {
                    IsSuccess = false,
                    Message = "Personnel not found"
                };

            _context.Personnel.Remove(persToDelete);
            await _context.SaveChangesAsync();

            return new ServiceResponce 
            {
                IsSuccess = true
            };
        }
    }
}
