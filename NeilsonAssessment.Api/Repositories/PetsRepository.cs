using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeilsonAssessment.Api.Entities;
using NeilsonAssessment.Api.Helpers;

namespace NeilsonAssessment.Api.Repositories
{
    public class PetsRepository : IPetsRepository
    {
        private MemoryCacheHelper _cache;
        private readonly string PetMemoryKey = "_Pets";

        public PetsRepository(MemoryCacheHelper memoryCache)
        {
            _cache = memoryCache;

            if (_cache.CacheTryGetValue(PetMemoryKey) == null)
            {
                var items = new List<Pet>()
                {
                    new Pet { Id = 1, Name = "Tom", Dob = new DateTime(2015, 1, 1), Sex = "Male", Breed = "German Shepard" },
                    new Pet { Id = 2, Name = "Jerry", Dob = new DateTime(2015, 1, 1), Sex = "Male", Breed = "Bulldog" },
                    new Pet { Id = 3, Name = "Max", Dob = new DateTime(2015, 1, 1), Sex = "Male", Breed = "Labrador Etriever" },
                    new Pet { Id = 4, Name = "Amber", Dob = new DateTime(2015, 1, 1), Sex = "Female", Breed = "Poodle" },
                    new Pet { Id = 5, Name = "Abbey", Dob = new DateTime(2015, 1, 1), Sex = "Female", Breed = "Boxer" },
                };

                _cache.CacheTrySetValue(PetMemoryKey, items);
            }
        }

        /// <summary>
        /// Gets all pets async.
        /// </summary>
        /// <returns>The all pets async.</returns>
        /// <param name="page">Page.</param>
        /// <param name="perPage">Per page.</param>
        public async Task<PagedList<Pet>> GetAllPetsAsync(int page, int perPage)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            return PagedList<Pet>.Create(items.AsQueryable(), page, perPage);
        }

        /// <summary>
        /// Gets the pet async.
        /// </summary>
        /// <returns>The pet async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<Pet> GetPetAsync(long id)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            Pet item = null;

            if (items != null)
            {
                item = items.SingleOrDefault(s => s.Id == id);
            }

            return item;
        }

        /// <summary>
        /// Inserts the pet async.
        /// </summary>
        /// <returns>The pet async.</returns>
        /// <param name="pet">Pet.</param>
        public async Task<Pet> InsertPetAsync(Pet pet)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            var maxId = items.Max(m => m.Id);
            pet.Id = maxId + 1;
            items.Add(pet);

            _cache.CacheTryUpdateValue(PetMemoryKey, items);

            return pet;
        }

        /// <summary>
        /// Updates the pet async.
        /// </summary>
        /// <returns>The pet async.</returns>
        /// <param name="pet">Pet.</param>
        public async Task UpdatePetAsync(Pet pet)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            var index = items.FindIndex(f => f.Id == pet.Id);

            items[index] = pet;

            _cache.CacheTryUpdateValue(PetMemoryKey, items);
        }

        /// <summary>
        /// Deletes the pet async.
        /// </summary>
        /// <returns>The pet async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task DeletePetAsync(long id)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            var index = items.FindIndex(f => f.Id == id);

            items.RemoveAt(index);

            _cache.CacheTryUpdateValue(PetMemoryKey, items);
        }

        /// <summary>
        /// Logs the pet gromming.
        /// </summary>
        /// <returns>The pet gromming.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="apponitmentDate">Apponitment date.</param>
        public async Task LogPetGromming(long id, DateTime apponitmentDate)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            var item = items.SingleOrDefault(s => s.Id == id);

            if (item.GrommingDates.Any(a => a.Equals(apponitmentDate))) throw new Exception("Pet gromming Appointment Date already exists");

            item?.GrommingDates.Add(apponitmentDate);

            _cache.CacheTryUpdateValue(PetMemoryKey, items);
        }

        /// <summary>
        /// Logs the veterinarian visit.
        /// </summary>
        /// <returns>The veterinarian visit.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="apponitmentDate">Apponitment date.</param>
        public async Task LogVeterinarianVisit(long id, DateTime apponitmentDate)
        {
            var items = await Task.FromResult(_cache.CacheTryGetValue(PetMemoryKey) as List<Pet>);

            var item = items.SingleOrDefault(s => s.Id == id);

            if (item.VeterinarianVisitDates.Any(a => a.Equals(apponitmentDate))) throw new Exception("Pet veterinarian visit Appointment Date already exists");

            item.VeterinarianVisitDates.Add(apponitmentDate);

            _cache.CacheTryUpdateValue(PetMemoryKey, items);
        }
    }
}
