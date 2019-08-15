using System;
using System.Threading.Tasks;
using NeilsonAssessment.Api.Entities;
using NeilsonAssessment.Api.Helpers;

namespace NeilsonAssessment.Api.Repositories
{
    public interface ICarsRepository
    {
        Task<PagedList<Car>> GetAllCarsAsync(int page, int perPage);
        Task<Car> GetCarAsync(long id);
        Task<Car> InsertCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(long id);
        Task LogCarWash(long id, DateTime apponitmentDate);
        Task LogMechanicVisit(long id, DateTime appointmentDate);
    }
}
