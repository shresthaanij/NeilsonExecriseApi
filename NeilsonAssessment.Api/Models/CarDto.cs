using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NeilsonAssessment.Api.Models
{
    public class CarDto: LinkedResourceBaseDto<EntityLinksDto>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Car Make is required")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Car Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Car Year is required")]
        public string Year { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "Car current milage is required")]
        public long CurrentMileage { get; set; }

        internal void PopulateLinks(IUrlHelper urlHelper)
        {
            _links = new EntityLinksDto()
            {
                Get = new LinkDto(urlHelper.Link("GetCarById", new { id = Id }), "get_car", "GET"),
                Update = new LinkDto(urlHelper.Link("UpdateCar", new { id = Id }), "update_car", "PUT"),
                Delete = new LinkDto(urlHelper.Link("DeleteCar", new { id = Id }), "delete_car", "DELETE")
            };
        }
    }
}
