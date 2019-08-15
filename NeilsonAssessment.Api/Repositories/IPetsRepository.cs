using System;
using System.Threading.Tasks;
using NeilsonAssessment.Api.Entities;
using NeilsonAssessment.Api.Helpers;

namespace NeilsonAssessment.Api.Repositories
{
    public interface IPetsRepository
    {
        Task DeletePetAsync(long id);
        Task<PagedList<Pet>> GetAllPetsAsync(int page, int perPage);
        Task<Pet> GetPetAsync(long id);
        Task<Pet> InsertPetAsync(Pet pet);
        Task LogPetGromming(long id, DateTime apponitmentDate);
        Task LogVeterinarianVisit(long id, DateTime apponitmentDate);
        Task UpdatePetAsync(Pet pet);
    }
}