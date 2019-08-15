using System;
using System.ComponentModel.DataAnnotations;

namespace NeilsonAssessment.Api.Models
{
    public class PetModel : LinkedResourceBaseDto<EntityLinksDto>
    {
        public PetModel()
        {
        }

        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Dob { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public string Breed { get; set; }
    }
}
