using System;
using System.Collections.Generic;

namespace NeilsonAssessment.Api.Entities
{
    public class Pet
    {
        public Pet()
        {
            GrommingDates = new List<DateTime>();
            VeterinarianVisitDates = new List<DateTime>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public string Sex { get; set; }
        public string Breed { get; set; }

        public List<DateTime> GrommingDates { get; set; }
        public List<DateTime> VeterinarianVisitDates { get; set; }
    }
}
