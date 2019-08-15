using System;
using System.Collections.Generic;

namespace NeilsonAssessment.Api.Entities
{
    public class Car
    {
        public Car()
        {
            CarWashDates = new List<DateTime>();
            MechanicVisitDates = new List<DateTime>();
        }

        public long Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public long CurrentMileage { get; set; }

        public List<DateTime> CarWashDates { get; set; }
        public List<DateTime> MechanicVisitDates { get; set; }
    }
}
