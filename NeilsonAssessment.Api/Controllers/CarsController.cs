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
    [Route("api/cars")]
    public class CarsController : Controller
    {
        private ILogger<CarsController> _logger;
        private readonly ICarsRepository _carsRepository;
        private readonly IUrlHelper _urlHelper;

        public CarsController(ILogger<CarsController> logger, ICarsRepository carsRepository, IUrlHelper urlHelper)
        {
            _logger = logger;
            _carsRepository = carsRepository;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetCars")]
        public async Task<IActionResult> Get(int page = 1, int perPage = 10)
        {
            if (page < 0) return BadRequest("page should be positive number only");

            if (perPage < 1 || perPage > 999) return BadRequest("perPage should be non zero and max of 999");

            var cars = await _carsRepository.GetAllCarsAsync(page, perPage);

            var carsToReturn = new EntityListDto<CarDto, Car>(cars, _urlHelper, "GetCars");
            carsToReturn.Items.ForEach(f => f.PopulateLinks(_urlHelper));

            return Ok(carsToReturn);
        }

        [HttpGet("{id}", Name = "GetCarById")]
        public async Task<IActionResult> Get(long id)
        {
            var car = await _carsRepository.GetCarAsync(id);

            if (car == null) return NotFound();

            var carToReturn = AutoMapper.Mapper.Map<CarDto>(car);
            carToReturn.PopulateLinks(_urlHelper);

            return Ok(carToReturn);
        }

        [HttpPost(Name = "CreateCar")]
        public async Task<IActionResult> Post([FromBody]CarDto model)
        {
            var car = await _carsRepository.InsertCarAsync(AutoMapper.Mapper.Map<Car>(model));

            return Ok();
        }

        [HttpPut("{id}", Name = "UpdateCar")]
        public async Task<IActionResult> Put(long id, [FromBody]CarDto model)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            if (model.Id == 0) model.Id = id;

            await _carsRepository.UpdateCarAsync(AutoMapper.Mapper.Map<Car>(model));

            return Ok();
        }

        [HttpDelete("{id}", Name = "DeleteCar")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            await _carsRepository.DeleteCarAsync(id);

            return Ok();
        }

        [HttpPost("{id}/carwash", Name = "ScheduleCarWash")]
        [Route("{id}/carwash")]
        public async Task<IActionResult> ScheduleCarWash(long id, [FromBody]CarWashModel model)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            await _carsRepository.LogCarWash(id, model.AppointmentDate);
            return Ok();
        }

        [HttpPost("{id}/mechanicvisit", Name = "ScheduleMechanicVisit")]
        [Route("{id}/mechanicvisit")]
        public async Task<IActionResult> ScheduleMechanicVisit(long id, [FromBody]MechanicVisitModel model)
        {
            if (!(await CheckCarExistsById(id))) return NotFound();

            await _carsRepository.LogMechanicVisit(id, model.AppointmentDate);
            return Ok();
        }

        private async Task<bool> CheckCarExistsById(long id)
        {
            var car = await _carsRepository.GetCarAsync(id);

            return car != null;
        }
    }
}
