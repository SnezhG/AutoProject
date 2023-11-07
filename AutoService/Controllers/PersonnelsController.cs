using Microsoft.AspNetCore.Mvc;
using AutoService.Models;
using AutoService.ServiceInterfaces;
using AutoService.DTO;

namespace AutoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelsController : ControllerBase
    {
        private readonly IPersonnelsService _personnelsService;

        public PersonnelsController(IPersonnelsService personnelsService)
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
        public async Task<IActionResult> PutPersonnel(int id, [FromBody] PersonnelDTO model)
        {
            var result = await _personnelsService.PutPersonnel(id, model);
            if (result.IsSuccess)
                return Ok();
            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> PostPersonnel([FromBody] PersonnelDTO model)
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
