using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeilsonAssessment.Api.Entities;
using NeilsonAssessment.Api.Helpers;

namespace NeilsonAssessment.Api.Repositories
{
    public class CarsRepository: ICarsRepository
    {
        private MemoryCacheHelper _cache;
        private readonly string CarMemoryKey = "_Cars";

        public CarsRepository(MemoryCacheHelper memoryCache)
        {
            _cache = memoryCache;

            if(_cache.CacheTryGetValue(CarMemoryKey) == null)
            {
                var items = new List<Car>()
                {
                    new Car { Id = 1, Make = "Audi", Model = "Q3", Year = "2018", CurrentMileage = 8522 },
                    new Car { Id = 2, Make = "Mercedes", Model = "C500", Year = "2018", CurrentMileage = 8324 },
                    new Car { Id = 3, Make = "Honda", Model = "CRV", Year = "2018", CurrentMileage = 9000 },
                    new Car { Id = 4, Make = "Honda", Model = "Accord", Year = "2018", CurrentMileage = 20000 },
                    new Car { Id = 5, Make = "Audi", Model = "Q3", Year = "2018", CurrentMileage = 5000 }
                };

                _cache.CacheTrySetValue(CarMemoryKey, items);
            }
        }

        /// <summary>
        /// Gets all cars async.
        /// </summary>
        /// <returns>The all cars async.</returns>
        /// <param name="page">Page.</param>
        /// <param name="perPage">Per page.</param>
        public async Task<PagedList<Car>> GetAllCarsAsync(int page, int perPage)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            return PagedList<Car>.Create(items.AsQueryable(), page, perPage);
        }

        /// <summary>
        /// Gets the car async.
        /// </summary>
        /// <returns>The car async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<Car> GetCarAsync(long id)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            Car item = null;

            if(items != null)
            {
                item = items.SingleOrDefault(s => s.Id == id);
            }

            return item;
        }

        /// <summary>
        /// Inserts the car async.
        /// </summary>
        /// <returns>The car async.</returns>
        /// <param name="car">Car.</param>
        public async Task<Car> InsertCarAsync(Car car)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            var maxId = items.Max(m => m.Id);
            car.Id = maxId + 1;
            items.Add(car);

            _cache.CacheTryUpdateValue(CarMemoryKey, items);

            return car;
        }

        /// <summary>
        /// Updates the car async.
        /// </summary>
        /// <returns>The car async.</returns>
        /// <param name="car">Car.</param>
        public async Task UpdateCarAsync(Car car)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            var index = items.FindIndex(f => f.Id == car.Id);

            items[index] = car;

            _cache.CacheTryUpdateValue(CarMemoryKey, items);
        }

        /// <summary>
        /// Deletes the car async.
        /// </summary>
        /// <returns>The car async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task DeleteCarAsync(long id)
        {
            var items =  await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            var index = items.FindIndex(f => f.Id == id);

            items.RemoveAt(index);

            _cache.CacheTryUpdateValue(CarMemoryKey, items);
        }

        /// <summary>
        /// Logs the car wash.
        /// </summary>
        /// <returns>The car wash.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="apponitmentDate">Apponitment date.</param>
        public async Task LogCarWash(long id, DateTime apponitmentDate)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            var item = items.SingleOrDefault(s => s.Id == id);

            if (item.CarWashDates.Any(a => a.Equals(apponitmentDate))) throw new Exception("Car wash Appointment Date already exists");

            item?.CarWashDates.Add(apponitmentDate);

            _cache.CacheTryUpdateValue(CarMemoryKey, items);
        }

        /// <summary>
        /// Logs the mechanic visit.
        /// </summary>
        /// <returns>The mechanic visit.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="apponitmentDate">Apponitment date.</param>
        public async Task LogMechanicVisit(long id, DateTime apponitmentDate)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(CarMemoryKey) as List<Car>);

            var item = items.SingleOrDefault(s => s.Id == id);

            if (item.CarWashDates.Any(a => a.Equals(apponitmentDate))) throw new Exception("Car Mechanic visit Appointment Date already exists");

            item.CarWashDates.Add(apponitmentDate);

            _cache.CacheTryUpdateValue(CarMemoryKey, items);
        }
    }
}
