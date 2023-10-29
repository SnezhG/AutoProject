using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.Data;
using AutoService.Models;
using AutoService.ViewModels;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelsController : ControllerBase
    {
        private readonly AutoContext _context;

        public PersonnelsController(AutoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personnel>>> GetPersonnel()
        {
          if (_context.Personnel == null)
          {
              return NotFound();
          }
            return await _context.Personnel.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Personnel>> GetPersonnel(int id)
        {
          if (_context.Personnel == null)
          {
              return NotFound();
          }
            var personnel = await _context.Personnel.FindAsync(id);

            if (personnel == null)
            {
                return NotFound();
            }

            return personnel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnel(int id, [FromBody] PersonnelViewModel model)
        {
            var persToEdit = await _context.Personnel.FindAsync(id);

            if (persToEdit == null)
                return NotFound("Cannot find person");

            persToEdit.Surname = model.Surname;
            persToEdit.Name = model.Name;
            persToEdit.Patronimyc = model.Patr;
            persToEdit.Post = model.Post;
            persToEdit.Experience = model.Exp;
            persToEdit.Available = model.Avail;

            _context.Personnel.Update(persToEdit);

            await _context.SaveChangesAsync();

            return Ok("Persons info updated successfuly");
        }

        [HttpPost]
        public async Task<ActionResult> PostPersonnel([FromBody] PersonnelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var persToCreate = new Personnel
                {
                    Surname = model.Surname,
                    Name = model.Name,
                    Patronimyc = model.Patr,
                    Post = model.Post,
                    Experience = model.Exp,
                    Available = model.Avail
                };

                _context.Personnel.Add(persToCreate);
                await _context.SaveChangesAsync();

                return Ok("Personnel created successfully");
            };

            return BadRequest("Some properties are incorrect");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnel(int id)
        {
            var persToDelete = await _context.Personnel.FindAsync(id);
            if (persToDelete == null)
                return NotFound("Personnel not found");

            _context.Personnel.Remove(persToDelete);
            await _context.SaveChangesAsync();

            return Ok("Bus deleted successfuly");
        }
    }
}
