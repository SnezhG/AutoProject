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
using AutoService.Services;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelsController : ControllerBase
    {
        private readonly PersonnelsService _personnelsService;

        public PersonnelsController(PersonnelsService personnelsService)
        {
            _personnelsService = personnelsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personnel>>> GetPersonnel()
        {
            var personnels = await _personnelsService.GetPersonnels();
            if (personnels == null)
                return NotFound();
            return Ok(personnels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Personnel>> GetPersonnel(int id)
        {
            var personnel = await _personnelsService.GetPersonnel(id);
            if (personnel == null)
                return NotFound();
            return Ok(personnel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonnel(int id, [FromBody] PersonnelViewModel model)
        {
            var result = await _personnelsService.PutPersonnel(id, model);
            if (result.IsSuccess)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> PostPersonnel([FromBody] PersonnelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _personnelsService.PostPersonnel(model);
                if (result.IsSuccess)
                    return Ok();
                return BadRequest();
            };

            return BadRequest("Some properties are incorrect");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonnel(int id)
        {
            var result = await _personnelsService.DeletePersonnel(id);
            if (result.IsSuccess)
                return Ok();
            return NotFound();
        }
    }
}
