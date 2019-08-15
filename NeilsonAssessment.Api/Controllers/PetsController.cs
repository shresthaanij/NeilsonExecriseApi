using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeilsonAssessment.Api.Entities;
using NeilsonAssessment.Api.Models;
using NeilsonAssessment.Api.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NeilsonAssessment.Api.Controllers
{
    [Route("api/pets")]
    public class PetsController : Controller
    {
        private ILogger<PetsController> _logger;
        private readonly IPetsRepository _petsRepository;
        private readonly IUrlHelper _urlHelper;

        public PetsController(ILogger<PetsController> logger, IPetsRepository petsRepository, IUrlHelper urlHelper)
        {
            _logger = logger;
            _petsRepository = petsRepository;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetPets")]
        public async Task<IActionResult> Get(int page = 1, int perPage = 10)
        {
            if (page < 0) return BadRequest("page should be positive number only");

            if (perPage < 1 || perPage > 999) return BadRequest("perPage should be non zero and max of 999");

            var pets = await _petsRepository.GetAllPetsAsync(page, perPage);

            var petsToReturn = new EntityListDto<PetDto, Pet>(pets, _urlHelper, "GetPets");
            petsToReturn.Items.ForEach(f => f.PopulateLinks(_urlHelper));

            return Ok(petsToReturn);
        }

        [HttpGet("{id}", Name = "GetPetById")]
        public async Task<IActionResult> Get(long id)
        {
            var pet = await _petsRepository.GetPetAsync(id);

            if (pet == null) return NotFound();

            var petToReturn = AutoMapper.Mapper.Map<PetDto>(pet);
            petToReturn.PopulateLinks(_urlHelper);

            return Ok(petToReturn);
        }

        [HttpPost(Name = "CreatePet")]
        public async Task<IActionResult> Post([FromBody]PetDto model)
        {
            var car = await _petsRepository.InsertPetAsync(AutoMapper.Mapper.Map<Pet>(model));

            return Ok();
        }

        [HttpPut("{id}", Name = "UpdatePet")]
        public async Task<IActionResult> Put(long id, [FromBody]CarDto model)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            if (model.Id == 0) model.Id = id;

            await _petsRepository.UpdatePetAsync(AutoMapper.Mapper.Map<Pet>(model));

            return Ok();
        }

        [HttpDelete("{id}", Name = "DeletePet")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            await _petsRepository.DeletePetAsync(id);

            return Ok();
        }

        [HttpPost("{id}/gromming", Name = "ScheduleGromming")]
        [Route("{id}/gromming")]
        public async Task<IActionResult> ScheduleGromming(long id, [FromBody]PetGrommingModel model)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            await _petsRepository.LogPetGromming(id, model.AppointmentDate);
            return Ok();
        }

        [HttpPost("{id}/veterinarianvisit", Name = "ScheduleVeterinarianVisit")]
        [Route("{id}/veterinarianvisit")]
        public async Task<IActionResult> ScheduleMechanicVisit(long id, [FromBody]VeterinarianVisitModel model)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            await _petsRepository.LogVeterinarianVisit(id, model.AppointmentDate);
            return Ok();
        }

        private async Task<bool> CheckCarExistsById(long id)
        {
            var car = await _petsRepository.GetPetAsync(id);

            return car != null;
        }
    }
}
