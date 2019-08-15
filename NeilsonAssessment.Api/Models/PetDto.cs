using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NeilsonAssessment.Api.Models
{
    public class PetDto : LinkedResourceBaseDto<EntityLinksDto>
    {
        public PetDto()
        {
        }

        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Dob { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public string Breed { get; set; }

        internal void PopulateLinks(IUrlHelper urlHelper)
        {
            _links = new EntityLinksDto()
            {
                Get = new LinkDto(urlHelper.Link("GetPetById", new { id = Id }), "get_pet", "GET"),
                Update = new LinkDto(urlHelper.Link("UpdatePet", new { id = Id }), "update_pet", "PUT"),
                Delete = new LinkDto(urlHelper.Link("DeletePet", new { id = Id }), "delete_pet", "DELETE")
            };
        }
    }
}
